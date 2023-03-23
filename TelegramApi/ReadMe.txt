Ngrok

--открыть туннель

ngrok http https://localhost:7005

ngrok http https://localhost:7005 --host-header="localhost:7005"

--inspect all incoming requests
http://127.0.0.1:4040/inspect/http (ngrok)

------------
Set Webhook
https://api.telegram.org/bot{my_bot_token}/setWebhook?url={url_to_send_updates_to}

Get Webhook
https://api.telegram.org/bot{my_bot_token}/getWebhookInfo

Delete webhook
https://api.telegram.org/botTOKEN/setWebhook?remove

------------
Настройки на iis, чтобы апи запускался моментально:

(By default the module starts the process for the ASP.NET Core app when the first request arrives and restarts the app if it crashes.)

I. Install the Application Initialization Module on IIS

Add Roles and Features -> Server Roles -> Web Server -> Web Server -> Application Development -> Application Initialization must be installed

II. Configure the app pool

In IIS Manager, right click on the application pool under which the application runs and select “Advanced Settings”. Update the following values:

Set the .NET CLR version to v4.0.
Set start mode to “Always Running”.
Set Idle Time-Out (minutes) to 0.

III. Configure the IIS site
In IIS Manager, right click on the site for the application, select “Manage Website” -> “Advanced Settings” and set the “Preload Enabled” value to true.


--------

Internet Information Services (IIS) application pools can be periodically recycled to avoid unstable states that can lead to application crashes, hangs, or memory leaks. 

When it recycles IHostedService methods StartAsync and StopAsync are called.

if 
ApplicationPool ->  AdvancedSettings -> Recycling -> DisableOverlappedRecycle is set to false, then StartAsync() called first and then StopAsync() second which deletes webHook and
Bot is not working any more,

so
ApplicationPool ->  AdvancedSettings -> Recycling -> DisableOverlappedRecycle has to be set to true, then StopAsync() called first and StartAsync() second.  


-------
https://api.telegram.org/bot{token}/getUpdates
