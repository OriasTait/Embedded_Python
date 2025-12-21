#!/usr/bin/env python3

import argparse
from collections.abc import Callable
from functools import cache
from os import getenv
from pathlib import Path
from shutil import which, rmtree
from subprocess import run, CalledProcessError
from sys import platform
import mimetypes


def define_arguments() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Automate individually compressing files into optimally compressed 7zip archives."
    )
    parser.add_argument("files", help="Files to be compressed.", nargs="+", type=Path)
    parser.add_argument("-d", "--delete", help="Delete original files after they have been compressed.", action="store_true")
    parser.add_argument("-o", "--output", dest="output_directory", help="Path to output compressed archives into. Defaults to same location as original file.", type=Path)
    parser.add_argument("-r", "--recursive", help="Recursively enter directories and compress files inside. If not used, directories will be compressed.", action="store_true")
    parser.add_argument("-s", "--skip-directories", help="Skip compressing directories.", action="store_true")
    parser.add_argument("-z", "--7zip", dest="x7zip", help="Path to 7zip program.", type=Path)
    return parser.parse_args()


def delete_path(path: Path):
    if path.is_dir():
        rmtree(path)
    else:
        path.unlink()


@cache
def which_7zip() -> Path:
    if platform.startswith("win32"):
        programfiles: str = getenv("PROGRAMFILES") or ''
        localappdata: str = getenv("LOCALAPPDATA") or ''
        for path in (
            which('7z.exe'),
            which('7z'),
            f"{programfiles}\\7-Zip\\7z.exe",
            f"{localappdata}\\7-Zip\\7z.exe",
            "7z.exe"
        ):
            if path and Path(path).exists():
                return Path(path)
    else:
        for path in (
            which('7z'),
            which('7zip'),
        ):
            if path and Path(path).exists():
                return Path(path)
    raise FileNotFoundError("Could not find a 7zip installation.")


def get_mimetype(file: Path) -> str:
    if file.is_dir():
        return 'inode/directory'
    mimetype, _ = mimetypes.guess_type(file.name)
    return mimetype or 'application/octet-stream'


def is_archive(file: Path, mimetype: str = "") -> bool:
    if mimetype == "":
        mimetype = get_mimetype(file)
    archive_extensions = {'.7z', '.zip', '.rar', '.tar', '.gz', '.tgz', '.tar.gz'}
    if file.suffix.lower() in archive_extensions:
        return True
    archive_mimetypes = [
        "application/x-7z-compressed", 
        "application/vnd.rar",
        "application/x-tar",
        "application/x-compressed-tar",
        "application/zip",
    ]
    return mimetype in archive_mimetypes


def compress_file(
    file: Path, 
    mimetype: str = "", 
    x7zip: Path | None = None,
    output_directory: Path | None = None,
) -> Path:
    if mimetype == "":
        mimetype = get_mimetype(file)
    if output_directory is None:
        output_directory = Path(file.parent)
    archive_output: Path = Path(output_directory, f"{file.name}.7z").resolve()
    x7zip = x7zip or which_7zip()
    command_compress: list[Path | str] = [
        x7zip, 'a', '-t7z', '-m0=lzma2', '-mx=9', '-myx=9', '-mqs=on', '-mmt=2'
    ]
    command_compress_text: list[Path | str] = [x7zip, 'a', '-t7z', '-mm=ppmd']
    if mimetype and mimetype.startswith("text/"):
        command: list[Path | str] = command_compress_text
    else:
        command = command_compress
    if archive_output.exists():
        raise FileExistsError(f"Archive {archive_output} already exists.")
    returncode: int = run(
        [*command, archive_output, file.name],
        cwd=Path(file.parent),
    ).returncode
    if returncode > 0:
        raise CalledProcessError(returncode, [*command, archive_output, file])
    return archive_output


def parse_files(
    *files: Path,
    delete: bool = False,
    recursive: bool = False,
    skip_directories: bool = False,
    user_7zip_path: Path | None = None,
    output_directory: Path | None = None,
) -> None:
    x7zip: Path = user_7zip_path or which_7zip()
    file_tasks: tuple[Callable, ...] = tuple()
    def compress_target(file: Path, x7zip: Path, output_directory: Path | None) -> None:
        try:
            archive_output: Path = compress_file(
                file,
                x7zip=x7zip,
                output_directory=output_directory,
            )
            if delete and archive_output.exists():
                delete_path(file)
        except FileExistsError as error:
            print(str(error), "Skipping.")
    def expand_directory(file: Path) -> None:
        nonlocal file_tasks
        file_tasks += assemble_file_tasks(tuple(file.iterdir()))
    def skip_archive(file: Path) -> None:
        print(f"Skipping '{file}': Already an archive")
    def skip_directory() -> None:
        pass
    def report_file_not_found(file: Path) -> None:
        print(f"File '{file}' not found. Skipping.")
    def determine_file_task_function(file: Path) -> Callable:
        if not file.exists():
            return lambda: report_file_not_found(file)
        elif file.is_dir():
            if skip_directories:
                return lambda: skip_directory()
            elif recursive:
                return lambda: expand_directory(file)
        elif is_archive(file):
            return lambda: skip_archive(file)
        return lambda: compress_target(file, x7zip=x7zip, output_directory=output_directory)
    def assemble_file_tasks(files: tuple[Path, ...]) -> tuple[Callable, ...]:
        return tuple(map(determine_file_task_function, files))
    file_tasks = assemble_file_tasks(files)
    del files
    while file_tasks:
        file_tasks[0]()
        file_tasks = file_tasks[1:]


def main() -> None:
    args: argparse.Namespace = define_arguments()
    parse_files(
        *args.files,
        delete = args.delete,
        recursive = args.recursive,
        skip_directories = args.skip_directories,
        user_7zip_path = args.x7zip,
        output_directory = args.output_directory,
    )


if __name__ == "__main__":
    main()
