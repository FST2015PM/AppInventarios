package md591ed960df9c0cd5ab62b0f0537062ce8;


public class MenusEditItem
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("PueblosMagicos.Android.Inventario.MenusEditItem, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MenusEditItem.class, __md_methods);
	}


	public MenusEditItem () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MenusEditItem.class)
			mono.android.TypeManager.Activate ("PueblosMagicos.Android.Inventario.MenusEditItem, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
