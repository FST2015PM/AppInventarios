package md53fd388ed257ece170a8fa9579f8e80c8;


public class GroundOverlayDemoActivity
	extends android.support.v4.app.FragmentActivity
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.OnMapReadyCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_SwitchImage:(Landroid/view/View;)V:__export__\n" +
			"n_onMapReady:(Lcom/google/android/gms/maps/GoogleMap;)V:GetOnMapReady_Lcom_google_android_gms_maps_GoogleMap_Handler:Android.Gms.Maps.IOnMapReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"";
		mono.android.Runtime.register ("MapsSample.GroundOverlayDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GroundOverlayDemoActivity.class, __md_methods);
	}


	public GroundOverlayDemoActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GroundOverlayDemoActivity.class)
			mono.android.TypeManager.Activate ("MapsSample.GroundOverlayDemoActivity, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void switchImage (android.view.View p0)
	{
		n_SwitchImage (p0);
	}

	private native void n_SwitchImage (android.view.View p0);


	public void onMapReady (com.google.android.gms.maps.GoogleMap p0)
	{
		n_onMapReady (p0);
	}

	private native void n_onMapReady (com.google.android.gms.maps.GoogleMap p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
