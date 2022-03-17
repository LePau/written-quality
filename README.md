Demo here: https://written-quality.netlify.app/

# About

This project is primarily a demonstration of my approach to development using technology I am familiar with.  It will contain coding conventions, project organization, and nuances that may be unique to me.  The goal is for others to get a sense for my style, practices, and competency towards creating a usable "prod ready" solution to a new problem in a short time span.

# Why did I pick this problem?

I wanted to explore the available AI/NLP services provided by the Google Cloud Platform.  I am interested in the problem of using AI to help grade and guide us (humans) towards writing more meaningful and objective content, especially when it comes to the decisions we make regarding policy and current affairs.  The ultimate idea would be to identify or remove human bias that is typically passed along while delivering news or information.

My goal with this small project was to be able to classify content overall with two main qualities: neutrality and stoicism.  Where neutrality is giving minimal criticism or praise while discussing a subject and stoicism is the lack of emotion while doing it.

# What did I learn?

I learned that Google NLP provides some pretty basic AI and NLP services, which I believe are inferior to others like OpenAI.  The closest function for detecting bias and emotion is sentiment analysis.  Unfortunately, the data fed to this learned algorithm is too broad or just not the right application for my purposes.

For example, I fed it an article regarding Tom Brady and it had "better" sentiment and lower "emotional" ratings than a documentation piece regarding Ionic progress bars.  I got a bit distracted in this area, and eventually accepted that my timeframe to achieve my above goal would not allow for much more tinkering. So, I pivoted the application to sentiment and length of the article, while breaking down the sentiment for each sentence, so that the user can observe the algorithm the way I did.

# Technologies

Front end framework - Angular
Front end UI - Ionic
Back end service provider - Google Cloud Platform
Back end framework - .Net Core 5
Additional services - Dockerfile, GCP Cloud Run, GCP Natural Language Processing

There is no persistence, so no storage/DB technologies were used.

# Running locally

To run the angular app, run `npm start run` from the `written-quality/written-quality-angular-ion` folder.  You may need to first run `npm install`.

To run the dotnet API app, review and run `start.sh` from the `written-quality/written-quality-dotnet` folder.  This application uses GCP services, so you will need to setup an account, enable the appropriate services (natural
language processing), and setup 

If you need a working METADATA_SERVICE_KEY, please let me know.

# Testing

Run `dotnet test` from the solution directory (`written-quality/written-quality-dotnet`) to run all tests across all projects.  Or, run that command from an individual project folder to run tests for that project only.

Testing is a work in progress.

# Deploying

## Single Page Application

* [Create netlify account](https://app.netlify.com/signup)
* [Setup netlify](https://www.netlify.com/blog/2016/09/29/a-step-by-step-guide-deploying-on-netlify/)
* `netlify sites:create --name written-quality`
* Review and run `written-quality/written-quality-angular-ion/deploy.sh`

## Dotnet core API

* Create a Google cloud account
* Review the `written-quality/written-quality-dotnet/Dockerfile` file
* Review and run `written-quality/written-quality-dotnet/deploy.sh`, which in a nutshell sets some environment variables for runtime and deploys the `Dockerfile` using the gcloud SDK
* For the first deploy, grab the URL from the GCP console -> Cloud Run -> written-quality-dotnet page and insert it into the SPA's `environment.prod.ts` config, so that the SPA knows which URL to communicate with

## Some notes regarding secrets

Cloud secrets are used to store sensitive information without having to store them locally in a file.  Users who are building locally and deploying would need to be authenticated with gcloud to get access to these secrets.

To list secrets:

```
gcloud secrets list
```

To create a secret:

```
gcloud secrets create METADATA_SERVICE_KEY --replication-policy=automatic
echo "<secret>" | gcloud secrets versions add METADATA_SERVICE_KEY --data-file=-
```

To access a secret:

```
gcloud secrets versions access latest --secret=METADATA_SERVICE_KEY
```

# Roadmap

* Research
    * Summarize document: https://www.nuget.org/packages/OpenTextSummarizer/
    * AI: GCP NLP API
    * Account management: metamask
    * Document store: IPFS
* Tasks
    * Account setup
        * GCP
        * Metamask
        * Scraper
    * Project setup
        * Dotnet core
            * Install dotnet core
            * `mkdir written-quality-dotnet`
            * `dotnet sln new`
            * `mkdir WrittenQuality.Api`
            * `dotnet new webapi`
        * Angular and Ionic
            * Install angular and ionic
            * `ionic start written-quality-angular-ion sidemenu --type=angular`
            * `ng g c EditContent`
        * Deploy and test skeleton
    * Paste and analyze document
        * Entities, sentiment
        * Summarize
        * Length, structure
    * Add basic unit tests
    * Setup web scraping of a document
    * Add `About` page
    * Unfinished
        * Page highlighting
        * Add login via metamask
        * Add recent document history via IPFS (through metamask)

# Improvements for prod

## Security

* Setup CORS
* Add CSP and verify XSS protection
* Run down other OWASP items (x-frames, HSTS, sniffing, removing headers with TMI, etc...)
* Require login/authorization to perform actions (due to cost)

## Quality

* Add more unit tests, for front end and back
* Add integration tests
* Verify logging is where it needs to be (updates, external requests)
* Better user error requests
* Immutify backend models behind readonly properties
* Move configurable items like maxEmotion to configuration

## User experience

* Add better validation for URL field, content field
* Refactor main layout into sub-components
* Use router links for navigating (but keep view service for updating)
* Add more quality points to analyze: author, source, analyzing deep links, including tags and categorization, grammar, spelling, puncuation, emoji spam
* Add additional APIs for social media like reddit, so that a link to a post or a comment can be pasted and analyzed
* Use a more reliable service for scraping (better proxies, more well kept)

# Other questions

### Why WrittenDocument and not just Document?

Namespace issues and to be explicit in order to avoid confusion.  

### Why the name WrittenDocument, WrittenQuality, etc...?

Because naming is hard.

### Why don't endpoints follow REST (in the HATEOAS sense)?

I tended to see this as more of an RPC type app to start with.

### Why not host SPA on GCP storage?

When following a link with `/written-quality/index.html` it will load fine, but the angular router will redirect and remove `index.html` from the end.  Any refresh from the browser will confuse the user with a listing error.

So, `/written-quality/index.html` works, but not `/written/quality`.  For the latter, GCP cloud storage attempts to list the directory, rather than what it says it should be doing after setting and validating the configuration here: `https://cloud.google.com/storage/docs/gsutil/commands/web`.

Original steps:

* Update package.json build script to set base href path for cloud location: `"build": "ng build --prod --base-href /written-quality/"`
* Create cloud account
* [Create cloud bucket](https://cloud.google.com/storage/docs/hosting-static-website) with name `written-quality`
* [Make bucket public](https://cloud.google.com/storage/docs/access-control/making-data-public)
* [Setup cloud SDK account](https://stackoverflow.com/a/52728878/4978821)
* Set `index.html` as default page with `gsutil web set -m index.html -e index.html gs://written-quality`
* Run `npm run build`
* Run `gsutil rsync -R -c www/ gs://written-quality` to sync build with bucket
* Run `./written-quality-angular/deploy.sh` to deploy front end to cloud storage
