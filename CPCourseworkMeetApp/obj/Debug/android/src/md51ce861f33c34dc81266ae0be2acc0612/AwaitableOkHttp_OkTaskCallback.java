package md51ce861f33c34dc81266ae0be2acc0612;


public class AwaitableOkHttp_OkTaskCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.squareup.okhttp.Callback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFailure:(Lcom/squareup/okhttp/Request;Ljava/io/IOException;)V:GetOnFailure_Lcom_squareup_okhttp_Request_Ljava_io_IOException_Handler:Square.OkHttp.ICallbackInvoker, Square.OkHttp\n" +
			"n_onResponse:(Lcom/squareup/okhttp/Response;)V:GetOnResponse_Lcom_squareup_okhttp_Response_Handler:Square.OkHttp.ICallbackInvoker, Square.OkHttp\n" +
			"";
		mono.android.Runtime.register ("OkHttpClient.AwaitableOkHttp+OkTaskCallback, OkHttpClient, Version=2.4.2.0, Culture=neutral, PublicKeyToken=null", AwaitableOkHttp_OkTaskCallback.class, __md_methods);
	}


	public AwaitableOkHttp_OkTaskCallback () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AwaitableOkHttp_OkTaskCallback.class)
			mono.android.TypeManager.Activate ("OkHttpClient.AwaitableOkHttp+OkTaskCallback, OkHttpClient, Version=2.4.2.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onFailure (com.squareup.okhttp.Request p0, java.io.IOException p1)
	{
		n_onFailure (p0, p1);
	}

	private native void n_onFailure (com.squareup.okhttp.Request p0, java.io.IOException p1);


	public void onResponse (com.squareup.okhttp.Response p0)
	{
		n_onResponse (p0);
	}

	private native void n_onResponse (com.squareup.okhttp.Response p0);

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
