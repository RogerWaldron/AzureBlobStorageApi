# AzureBlobStorage Api

Api for Azure Blob Storage

## Features:

- File Downloading
- File Uploading
- File Deleting
- Getting all files in container

### Note

- all files are stored in container named "files"
- if "files" folder doesn't exist it will be created inside container with public permissions
- files are saved with same extension but a new name generated on Guid, for example:

  50bb5e5f-056d-4ade-afcb-f83655b71d1a.pdf

## Tech Stack

- C#
- DotNet 7
- Azure Blob Storage REST Api

## Requires:

- Azure Account
- Updating "StorageConnection" example key in appsetting.json with your Azure Blob Storage connection details

## Swagger endpoint

http://localhost:5217/swagger/index.html
