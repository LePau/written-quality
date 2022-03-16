#!/bin/bash

gcloud builds submit --tag gcr.io/astral-oven-344200/written-quality-dotnet

export METADATA_SERVICE_DOMAIN="https://demos-website-metadata-vjoja4wq7q-uw.a.run.app"
export METADATA_SERVICE_KEY=`gcloud secrets versions access latest --secret=METADATA_SERVICE_KEY`

# JSON files don't work with topic escaping, like below
# > gcloud topic escaping
# --dict-flag=^:^a=b,c:d=f,g # => {'a': 'b,c', 'd': 'f,g'}
# INSTEAD, need to just replace comma's with something, then will place them back on server side
# it's easier than using a YAML file or having to deal with files from disk
export GOOGLE_APPLICATION_CREDENTIALS=`gcloud secrets versions access latest --secret=GOOGLE_APPLICATION_CREDENTIALS`
GOOGLE_APPLICATION_CREDENTIALS=`echo $GOOGLE_APPLICATION_CREDENTIALS | sed 's/,/;;;/g'`

gcloud run deploy \
  --image gcr.io/astral-oven-344200/written-quality-dotnet \
  --platform managed written-quality-dotnet \
  --set-env-vars="GOOGLE_APPLICATION_CREDENTIALS_JSON=$GOOGLE_APPLICATION_CREDENTIALS" \
  --set-env-vars="GOOGLE_APPLICATION_CREDENTIALS=/tmp/google_application_credentials.json" \
  --set-env-vars="APP_NAME=WrittenQuality" \
  --set-env-vars="ASPNETCORE_ENVIRONMENT=Production" \
  --set-env-vars="IS_PROD=true" \
  --set-env-vars="METADATA_SERVICE_KEY=$METADATA_SERVICE_KEY" \
  --set-env-vars="METADATA_SERVICE_DOMAIN=$METADATA_SERVICE_DOMAIN" 

