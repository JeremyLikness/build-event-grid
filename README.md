# //build Event Grid

Event Grid publisher and consumer example.

* [Watch the related 20-minute video and live demo](https://jlik.me/dij)

## Portal setup 

1. In the Azure portal, create a new resource of type "Event Grid Topic." You can give it any name. 

## Environment setup

1. On a command line, set environment variables `EVENT_GRID_KEY` and `EVENT_GRID_URL`

    1. `export EVENT_GRID_URL={url}` where `{url}` is the topic endpoint on the overview tab for your topic 

    1. `export EVENT_GRID_KEY={key}` where `{key}` can be found in the portal under "Access Keys" for your topic 

## Set up the broadcaster

1. `git clone https://github.com/JeremyLikness/build-event-grid.sh`

1. `cd build-event-grid`

1. `cd broadcaster` 

1. `dotnet build -c Release` 

1. `dotnet run "This is a test."` should result in "OK" if your environment variables are set up correctly

## Set up the receiver 

1. The receiver is in the `receiver` folder and is a .NET Core Web API app 

    1. There is no special configuration required for the receiver app 

    1. You can deploy the receiver to a Web App, the only requirement is that it is secured by https 
    
    1. You can also build a Docker container (a `Dockerfile` is included) and publish that, again it must be secured
    
1. In the portal for your event grid topic, add a subscription of type "WebHook" and give it the URL: https://{yourdomain}/api/build 

1. Once the subscription is confirmed, you should see messages written to the web logs 

1. Send a message with a broadcaster and you can confirm it's receipt by inspecting the logs for the web app or docker instance

DM me with any questions: [@JeremyLikness](https://twitter.com/JeremyLikness) (my DMs are open).

