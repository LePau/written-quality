#!/bin/bash

export GOOGLE_APPLICATION_CREDENTIALS_JSON=`gcloud secrets versions access latest --secret=GOOGLE_APPLICATION_CREDENTIALS`
export GOOGLE_APPLICATION_CREDENTIALS="/tmp/google_application_credentials.json" # written by server on startup from respective *_JSON var
export METADATA_SERVICE_DOMAIN="https://demos-website-metadata-vjoja4wq7q-uw.a.run.app"
export METADATA_SERVICE_KEY=`gcloud secrets versions access latest --secret=METADATA_SERVICE_KEY`
export APP_NAME=WrittenQuality
export IS_PROD=false

dotnet watch --project=./WrittenQuality.Api/WrittenQuality.Api.csproj run

sleep 1

wait