package md591ed960df9c0cd5ab62b0f0537062ce8;


public class MercadosTextosActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"";
		mono.android.Runtime.register ("PueblosMagicos.Android.Inventario.MercadosTextosActivity, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MercadosTextosActivity.class, __md_methods);
	}


	public MercadosTextosActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MercadosTextosActivity.class)
			mono.android.TypeManager.Activate ("PueblosMagicos.Android.Inventario.MercadosTextosActivity, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
