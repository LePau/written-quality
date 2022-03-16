npm run build
# gsutil rsync -R -c www/ gs://written-quality

cp src/_robots.txt www/robots.txt
cp src/_redirects www/_redirects

# https://written-quality.netlify.app
netlify deploy --site=4c789b67-1883-4862-b71b-33376e429f9c --dir=./www/ --prod