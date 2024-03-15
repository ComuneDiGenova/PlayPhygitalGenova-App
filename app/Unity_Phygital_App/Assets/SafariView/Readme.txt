Usage:
	- Call SafariViewController.OpenURL(string url) to open an URL in embedded Safari web view.
	
Test:
	- Build the Example scene and run on iOS device.

Subscribe to events:
	- SafariViewController.DidCompleteInitialLoad(bool success)
	- SafariViewController.InitialLoadDidRedirectToURL(string url)
	- SafariViewController.ViewControllerDidFinish()