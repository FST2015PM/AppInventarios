package md53fd388ed257ece170a8fa9579f8e80c8;


public class DemoDetails
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MapsSample.DemoDetails, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DemoDetails.class, __md_methods);
	}


	public DemoDetails () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DemoDetails.class)
			mono.android.TypeManager.Activate ("MapsSample.DemoDetails, MapsSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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