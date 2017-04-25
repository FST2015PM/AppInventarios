package md5446c5f529714f04776e32938888c7c38;


public class Compass
	extends md5446c5f529714f04776e32938888c7c38.GraphicsActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"n_onStop:()V:GetOnStopHandler\n" +
			"";
		mono.android.Runtime.register ("PueblosMagicos.Android.Inventario.Graphics.Compass, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Compass.class, __md_methods);
	}


	public Compass () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Compass.class)
			mono.android.TypeManager.Activate ("PueblosMagicos.Android.Inventario.Graphics.Compass, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();


	public void onStop ()
	{
		n_onStop ();
	}

	private native void n_onStop ();

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
