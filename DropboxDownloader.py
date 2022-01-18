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

# This script is used to download files from a given dropbox directory.
# Files to be downloaded are filtered based on given date present in file name.

# ARGUMENTS
# DROPBOX_API_KEY: Dropbox API KEY with read access.
# DROPBOX_SOURCE_DIRECTORY: path of the dropbox directory to search files within.
# DROPBOX_OUTPUT_DIRECTORY(optional): base path of the output directory to store to downloaded files.
# cmdline args expected in order: DROPBOX_API_KEY, DROPBOX_SOURCE_DIRECTORY, QC_DATAFLEET_DEPLOYMENT_DATE, DROPBOX_OUTPUT_DIRECTORY

import requests
import json
import sys
import time
import os
from pathlib import Path

DROPBOX_API_KEY = os.environ.get("DROPBOX_API_KEY")
DROPBOX_SOURCE_DIRECTORY = os.environ.get("DROPBOX_SOURCE_DIRECTORY")
QC_DATAFLEET_DEPLOYMENT_DATE = os.environ.get("QC_DATAFLEET_DEPLOYMENT_DATE")
DROPBOX_OUTPUT_DIRECTORY = os.environ.get("DROPBOX_OUTPUT_DIRECTORY", "/raw")

def DownloadZipFile(filePath):

	print(f"Starting downloading file at: {filePath}") 

	# defining the api-endpoint
	API_ENDPOINT_DOWNLOAD = "https://content.dropboxapi.com/2/files/download"
	
	# data to be sent to api
	data = {"path": filePath}

	headers = {"Authorization": f"Bearer {DROPBOX_API_KEY}",
				"Dropbox-API-Arg": json.dumps(data)}

	# sending post request and saving response as response object
	response = requests.post(url = API_ENDPOINT_DOWNLOAD, headers=headers)

	response.raise_for_status() # ensure we notice bad responses

	fileName = filePath.split("/")[-1]
	outputPath = os.path.join(DROPBOX_OUTPUT_DIRECTORY, fileName)
	
	with open(outputPath, "wb") as f:
		f.write(response.content)
	print(f"Succesfully saved file at: {outputPath}")

def GetFilePathsFromDate(targetLocation, dateString):
	# defining the api-endpoint 
	API_ENDPOINT_FILEPATH = "https://api.dropboxapi.com/2/files/list_folder"

	headers = {"Content-Type": "application/json",
				"Authorization": f"Bearer {DROPBOX_API_KEY}"}
	
	# data to be sent to api
	data = {"path": targetLocation,
			"recursive": False,
			"include_media_info": False,
			"include_deleted": False,
			"include_has_explicit_shared_members": False,
			"include_mounted_folders": True,
			"include_non_downloadable_files": True}
	
	# sending post request and saving response as response object
	response = requests.post(url = API_ENDPOINT_FILEPATH, headers=headers, data = json.dumps(data))
	
	response.raise_for_status() # ensure we notice bad responses

	target_paths = [entry["path_display"] for entry in response.json()["entries"] if dateString in entry["path_display"]]
	return target_paths

def main():
	global DROPBOX_API_KEY, DROPBOX_SOURCE_DIRECTORY, QC_DATAFLEET_DEPLOYMENT_DATE, DROPBOX_OUTPUT_DIRECTORY
	inputCount = len(sys.argv)
	if inputCount > 1:
		DROPBOX_API_KEY = sys.argv[1]
	if inputCount > 2:	
		DROPBOX_SOURCE_DIRECTORY = sys.argv[2]
	if inputCount > 3:
		QC_DATAFLEET_DEPLOYMENT_DATE = sys.argv[3]
	if inputCount > 4:
		DROPBOX_OUTPUT_DIRECTORY = sys.argv[4]

	# make output path if doesn't exists
	Path(DROPBOX_OUTPUT_DIRECTORY).mkdir(parents=True, exist_ok=True)
	
	target_paths = GetFilePathsFromDate(DROPBOX_SOURCE_DIRECTORY, QC_DATAFLEET_DEPLOYMENT_DATE)
	print(f"Found {len(target_paths)} files with following paths {target_paths}")

	#download files
	for path in target_paths:
		count = 0
		maxTries = 3
		while True:	
			try:
				DownloadZipFile(path)
				break
			except Exception as e:
				count +=1
				if count > maxTries:
					print(f"Error for file with path {path} --error message: {e}")
					break
				print(f"Error, sleep for 5 sec and retry download file with --path: {path}")
				time.sleep(5)

if __name__== "__main__":
	main()
