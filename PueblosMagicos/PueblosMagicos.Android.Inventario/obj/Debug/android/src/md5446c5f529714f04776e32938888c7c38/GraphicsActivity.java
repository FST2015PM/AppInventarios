package md5446c5f529714f04776e32938888c7c38;


public class GraphicsActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_setContentView:(Landroid/view/View;)V:GetSetContentView_Landroid_view_View_Handler\n" +
			"";
		mono.android.Runtime.register ("PueblosMagicos.Android.Inventario.Graphics.GraphicsActivity, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GraphicsActivity.class, __md_methods);
	}


	public GraphicsActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GraphicsActivity.class)
			mono.android.TypeManager.Activate ("PueblosMagicos.Android.Inventario.Graphics.GraphicsActivity, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void setContentView (android.view.View p0)
	{
		n_setContentView (p0);
	}

	private native void n_setContentView (android.view.View p0);

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
