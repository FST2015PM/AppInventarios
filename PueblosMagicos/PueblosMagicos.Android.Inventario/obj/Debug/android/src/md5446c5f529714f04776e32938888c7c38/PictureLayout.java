package md5446c5f529714f04776e32938888c7c38;


public class PictureLayout
	extends android.view.ViewGroup
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_addView:(Landroid/view/View;)V:GetAddView_Landroid_view_View_Handler\n" +
			"n_addView:(Landroid/view/View;I)V:GetAddView_Landroid_view_View_IHandler\n" +
			"n_addView:(Landroid/view/View;ILandroid/view/ViewGroup$LayoutParams;)V:GetAddView_Landroid_view_View_ILandroid_view_ViewGroup_LayoutParams_Handler\n" +
			"n_addView:(Landroid/view/View;Landroid/view/ViewGroup$LayoutParams;)V:GetAddView_Landroid_view_View_Landroid_view_ViewGroup_LayoutParams_Handler\n" +
			"n_generateDefaultLayoutParams:()Landroid/view/ViewGroup$LayoutParams;:GetGenerateDefaultLayoutParamsHandler\n" +
			"n_onMeasure:(II)V:GetOnMeasure_IIHandler\n" +
			"n_dispatchDraw:(Landroid/graphics/Canvas;)V:GetDispatchDraw_Landroid_graphics_Canvas_Handler\n" +
			"n_invalidateChildInParent:([ILandroid/graphics/Rect;)Landroid/view/ViewParent;:GetInvalidateChildInParent_arrayILandroid_graphics_Rect_Handler\n" +
			"n_onLayout:(ZIIII)V:GetOnLayout_ZIIIIHandler\n" +
			"";
		mono.android.Runtime.register ("PueblosMagicos.Android.Inventario.Graphics.PictureLayout, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PictureLayout.class, __md_methods);
	}


	public PictureLayout (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == PictureLayout.class)
			mono.android.TypeManager.Activate ("PueblosMagicos.Android.Inventario.Graphics.PictureLayout, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public PictureLayout (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == PictureLayout.class)
			mono.android.TypeManager.Activate ("PueblosMagicos.Android.Inventario.Graphics.PictureLayout, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public PictureLayout (android.content.Context p0, android.util.AttributeSet p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == PictureLayout.class)
			mono.android.TypeManager.Activate ("PueblosMagicos.Android.Inventario.Graphics.PictureLayout, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public PictureLayout (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3) throws java.lang.Throwable
	{
		super (p0, p1, p2, p3);
		if (getClass () == PictureLayout.class)
			mono.android.TypeManager.Activate ("PueblosMagicos.Android.Inventario.Graphics.PictureLayout, PueblosMagicos.Android.Inventario, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void addView (android.view.View p0)
	{
		n_addView (p0);
	}

	private native void n_addView (android.view.View p0);


	public void addView (android.view.View p0, int p1)
	{
		n_addView (p0, p1);
	}

	private native void n_addView (android.view.View p0, int p1);


	public void addView (android.view.View p0, int p1, android.view.ViewGroup.LayoutParams p2)
	{
		n_addView (p0, p1, p2);
	}

	private native void n_addView (android.view.View p0, int p1, android.view.ViewGroup.LayoutParams p2);


	public void addView (android.view.View p0, android.view.ViewGroup.LayoutParams p1)
	{
		n_addView (p0, p1);
	}

	private native void n_addView (android.view.View p0, android.view.ViewGroup.LayoutParams p1);


	public android.view.ViewGroup.LayoutParams generateDefaultLayoutParams ()
	{
		return n_generateDefaultLayoutParams ();
	}

	private native android.view.ViewGroup.LayoutParams n_generateDefaultLayoutParams ();


	public void onMeasure (int p0, int p1)
	{
		n_onMeasure (p0, p1);
	}

	private native void n_onMeasure (int p0, int p1);


	public void dispatchDraw (android.graphics.Canvas p0)
	{
		n_dispatchDraw (p0);
	}

	private native void n_dispatchDraw (android.graphics.Canvas p0);


	public android.view.ViewParent invalidateChildInParent (int[] p0, android.graphics.Rect p1)
	{
		return n_invalidateChildInParent (p0, p1);
	}

	private native android.view.ViewParent n_invalidateChildInParent (int[] p0, android.graphics.Rect p1);


	public void onLayout (boolean p0, int p1, int p2, int p3, int p4)
	{
		n_onLayout (p0, p1, p2, p3, p4);
	}

	private native void n_onLayout (boolean p0, int p1, int p2, int p3, int p4);

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
