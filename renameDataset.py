#!/usr/bin/env python3
#
# QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
# Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#

"""
Renames the SDK template classes, files, and references to match a real dataset.

The dataset name is derived from the repo folder name (everything after the last dot).
For example, if the repo is cloned as "Lean.DataSource.MyVendorData", the name is "MyVendorData".

Usage:
    python renameDataset.py              # derives name from folder
    python renameDataset.py MyVendorData # explicit name
    python renameDataset.py --dry-run    # preview changes without applying
"""

import os
import sys
from pathlib import Path


def replace_in_file(filepath: Path, replacements: list[tuple[str, str]]) -> bool:
    """Replace all occurrences in a file. Returns True if any changes were made."""
    try:
        text = filepath.read_text(encoding="utf-8-sig")
    except (UnicodeDecodeError, FileNotFoundError):
        return False

    original = text
    for old, new in replacements:
        text = text.replace(old, new)

    if text != original:
        filepath.write_text(text, encoding="utf-8")
        return True
    return False


def rename_file(src: Path, dst: Path, dry_run: bool) -> None:
    """Rename a file if it exists."""
    if not src.exists():
        print(f"  SKIP (not found): {src}")
        return
    if dst.exists():
        print(f"  SKIP (target exists): {dst}")
        return
    print(f"  {src.relative_to(root)} -> {dst.relative_to(root)}")
    if not dry_run:
        src.rename(dst)


def main():
    global root
    root = Path(__file__).resolve().parent

    dry_run = "--dry-run" in sys.argv
    args = [a for a in sys.argv[1:] if not a.startswith("--")]

    if args:
        name = args[0]
    else:
        name = root.name.rsplit(".", 1)[-1]

    if name in ("SDK", "DataSource") or not name[0].isupper():
        print(f"Error: '{name}' doesn't look like a valid dataset name.")
        print("Usage: python renameDataset.py VendorNameDatasetName")
        sys.exit(1)

    universe_name = f"{name}Universe"
    algorithm_name = f"{name}Algorithm"
    universe_algo_name = f"{name}UniverseSelectionAlgorithm"
    downloader_name = f"{name}DataDownloader"
    provider_name = f"{name}DataProvider"
    name_lower = name.lower()

    print(f"Dataset name: {name}")
    print(f"Universe:     {universe_name}")
    print(f"Algorithm:    {algorithm_name}")
    print(f"Downloader:   {downloader_name}")
    print(f"Provider:     {provider_name}")
    if dry_run:
        print("(DRY RUN - no changes will be made)\n")
    else:
        print()

    # --- Text replacements ---
    # Order matters: longer/more-specific patterns first to avoid partial matches
    replacements = [
        ("MyCustomDataQueueHandler", f"{name}DataQueueHandler"),
        ("MyCustomDataDownloader", downloader_name),
        ("MyCustomDataProvider", provider_name),
        ("MyCustomDataUniverse", universe_name),
        ("MyCustomDataType", name),
        ("MyCustomData", name),
        ("MyCustom", name),
        ("CustomDataAlgorithm", algorithm_name),
        ("CustomDataUniverse", universe_algo_name),
        ("DemonstrationUniverse", universe_algo_name),
        ("Demonstration", algorithm_name),
        ("mycustomdatatype", name_lower),
        ("mycustomdata", name_lower),
    ]

    # Files to apply text replacements to (relative to root)
    target_files = [
        "QuantConnect.DataSource.csproj",
        "MyCustomDataType.cs",
        "MyCustomDataUniverse.cs",
        "MyCustomDataDownloader.cs",
        "MyCustomDataProvider.cs",
        "mycustomdata.json",
        "Demonstration.cs",
        "Demonstration.py",
        "DemonstrationUniverse.cs",
        "DemonstrationUniverse.py",
        "tests/MyCustomDataTypeTests.cs",
        "tests/MyCustomDataDownloaderTests.cs",
        "tests/MyCustomDataProviderHistoryTests.cs",
        "tests/MyCustomDataQueueHandlerTests.cs",
        "tests/Tests.csproj",
        "DataProcessing/MyCustomDataDownloader.cs",
        "DataProcessing/Program.cs",
    ]

    print("Text replacements:")
    for rel in target_files:
        filepath = root / rel
        if not filepath.exists():
            continue
        if dry_run:
            try:
                text = filepath.read_text(encoding="utf-8-sig")
            except (UnicodeDecodeError, FileNotFoundError):
                continue
            changed = any(old in text for old, _ in replacements)
            if changed:
                print(f"  {rel}")
        else:
            if replace_in_file(filepath, replacements):
                print(f"  {rel}")

    # --- File renames ---
    print("\nFile renames:")
    file_renames = [
        ("MyCustomDataType.cs", f"{name}.cs"),
        ("MyCustomDataUniverse.cs", f"{universe_name}.cs"),
        ("MyCustomDataDownloader.cs", f"{downloader_name}.cs"),
        ("MyCustomDataProvider.cs", f"{provider_name}.cs"),
        ("mycustomdata.json", f"{name_lower}.json"),
        ("Demonstration.cs", f"{algorithm_name}.cs"),
        ("Demonstration.py", f"{algorithm_name}.py"),
        ("DemonstrationUniverse.cs", f"{universe_algo_name}.cs"),
        ("DemonstrationUniverse.py", f"{universe_algo_name}.py"),
        ("tests/MyCustomDataTypeTests.cs", f"tests/{name}Tests.cs"),
        ("tests/MyCustomDataDownloaderTests.cs", f"tests/{downloader_name}Tests.cs"),
        (
            "tests/MyCustomDataProviderHistoryTests.cs",
            f"tests/{provider_name}HistoryTests.cs",
        ),
        (
            "tests/MyCustomDataQueueHandlerTests.cs",
            f"tests/{name}DataQueueHandlerTests.cs",
        ),
        (
            "DataProcessing/MyCustomDataDownloader.cs",
            f"DataProcessing/{downloader_name}.cs",
        ),
    ]

    for old_rel, new_rel in file_renames:
        rename_file(root / old_rel, root / new_rel, dry_run)

    # --- Directory renames (sample data) ---
    print("\nDirectory renames:")
    dir_renames = [
        ("output/alternative/mycustomdatatype", f"output/alternative/{name_lower}"),
    ]
    for old_rel, new_rel in dir_renames:
        rename_file(root / old_rel, root / new_rel, dry_run)

    if dry_run:
        print("\nDry run complete. Re-run without --dry-run to apply.")
    else:
        print("\nDone! You may want to rename the repo folder and .sln file manually.")


if __name__ == "__main__":
    main()
