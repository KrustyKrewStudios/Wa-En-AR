mergeInto(LibraryManager.library, {
    WebGLPlaceOrigin: function(camPosStr)
    {
        window.wTracker.placeOrigin(UTF8ToString(camPosStr));
    },
    WebGLResetOrigin: function()
    {
        window.wTracker.resetOrigin();
    },
    StartWebGLwTracker: function(name)
	{
        if(!window.wTracker){
            console.error('%cwTracker not found! Please make sure to use the wTracker WebGLTemplate in your ProjectSettings','font-size: 32px; font-weight: bold');
            throw new Error("Tracker not found! Please make sure to use the wTracker WebGLTemplate in your ProjectSettings");
            return;
        }

    	window.wTracker.startTracker(UTF8ToString(name));
    },
    StopWebGLwTracker: function()
	{
    	window.wTracker.stopTracker();
    },
    IsWebGLwTrackerReady: function()
    {
        return window.wTracker != null;
    },
    SetWebGLwTrackerSettings: function(settings)
	{
    	window.wTracker.setTrackerSettings(UTF8ToString(settings),"1.4.11.590507");
    },
    WebGLSyncSSPos: function(vStr){
        window.wTracker.syncSSPos(UTF8ToString(vStr));
    },

    WebGLGetGPSPosition: function(){
        window.wTracker.getGPSPosition();
    },
    WebGLSubscribeToGPSPositionUpdates: function(){
        window.wTracker.subscribeToGPSPositionChanges();
    },
    WebGLUnsubscribeToGPSPositionUpdates: function(){
        window.wTracker.unsubscribeToGPSPositionChanges();
    },
}); 