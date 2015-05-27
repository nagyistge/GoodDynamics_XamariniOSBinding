using System;

using ObjCRuntime;
using Foundation;
using UIKit;
using CoreData;

namespace GoodDynamics {

	[BaseType (typeof (NSObject))]
	public interface GDAppDetail {

		[Export ("applicationId", ArgumentSemantic.Retain)]
		string ApplicationId { get; set; }

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		[Export ("address", ArgumentSemantic.Retain)]
		string Address { get; set; }

		[Export ("versionId", ArgumentSemantic.Retain)]
		string VersionId { get; set; }
	}

	[BaseType (typeof(NSObject))]
	public interface GDServiceProvider
	{
		[Export("identifier", ArgumentSemantic.Retain)]
		string Identifier { get; set;}

		[Export("version", ArgumentSemantic.Retain)]
		string Version {get;set;}

		[Export("name", ArgumentSemantic.Retain)]
		string Name {get;set;}

		[Export("address", ArgumentSemantic.Retain)]
		string Address {get;set;}

		[Export("icon", ArgumentSemantic.Retain)]
		UIImage Icon {get;set;}

		[Export("iconPending", ArgumentSemantic.Retain)]
		bool IconPending {get;set;}

		[Export("serverCluster", ArgumentSemantic.Retain)]
		NSArray ServerCluster {get;set;}

		[Export("services", ArgumentSemantic.Retain)]
		NSArray Services {get;set;}
	}
	
	public delegate void SendFileSuccessBlock(NSError error);
	
	[BaseType (typeof (NSObject))]
	public interface GDAppServer {

		[Export ("initWithServer:andPort:andPriority:")]
		IntPtr Constructor (string server, NSNumber port, NSNumber priority);

		[Export ("server", ArgumentSemantic.Retain)]
		string Server { get; }

		[Export ("port", ArgumentSemantic.Retain)]
		NSNumber Port { get; }

		[Export ("priority", ArgumentSemantic.Retain)]
		NSNumber Priority { get; }
	}

	[BaseType(typeof(NSInputStream))]
	public interface GDCReadStream {

		[Export ("initWithFile:")]
		IntPtr Constructor (string filePath);

		[Export ("seekToFileOffset:")]
		bool SeekToFileOffset (ulong offset);

		[Export ("streamError"),]
		NSError StreamError { get; }
	}

	[BaseType(typeof(NSOutputStream))]
	public interface GDCWriteStream{

		[Export ("initWithFile:append:")]
		IntPtr Constructor (string filePath, bool shouldAppend);

		[Export ("streamError")]
		NSError StreamError { get; }
	}

	[BaseType (typeof (NSPersistentStoreCoordinator))]
	public interface GDPersistentStoreCoordinator {

		[Field ("GDEncryptedBinaryStoreType", "__Internal")]
		NSString GDEncryptedBinaryStoreType { get; }

		[Field ("GDEncryptedBinaryStoreErrorDomain", "__Internal")]
		NSString GDEncryptedBinaryStoreErrorDomain { get; }

		[Field ("GDEncryptedIncrementalStoreType", "__Internal")]
		NSString GDEncryptedIncrementalStoreType { get; }

		[Field ("GDEncryptedIncrementalStoreErrorDomain", "__Internal")]
		NSString GDEncryptedIncrementalStoreErrorDomain { get; }

		[Field ("GDFileSystemErrorDomain", "__Internal")]
		NSString GDFileSystemErrorDomain { get; }
	}

	[BaseType(typeof(NSObject))]
	public interface GDFileSystem {

		[Static, Export ("moveFileToSecureContainer:error:")]
		bool MoveFileToSecureContainer (string absoluteFilenameWithPath, out NSError error);

		[Static, Export ("getFileStat:to:error:")]
		bool GetFileStat (string filePath, ref GDFileStat filestat, out NSError error);

		[Static, Export ("removeItemAtPath:error:")]
		bool RemoveItemAtPath (string filePath, out NSError error);

		[Static, Export ("getReadStream:error:")]
		GDCReadStream GetReadStream (string filePath, out NSError error);

		[Static, Export ("getWriteStream:appendmode:error:")]
		GDCWriteStream GetWriteStream (string filePath, bool flag, out NSError error);

		[Static, Export ("readFromFile:error:")]
		NSData ReadFromFile (string name, out NSError error);

		[Static, Export ("writeToFile:name:error:")]
		bool WriteToFile (NSData data, string name, out NSError error);

		[Static, Export ("writeToFile:name:fromOffset:error:")]
		bool WriteToFile (NSData data, string name, int offset, out NSError error);

		[Static, Export ("createDirectoryAtPath:withIntermediateDirectories:attributes:error:")]
		bool CreateDirectoryAtPath (string path, bool createIntermediates, NSDictionary attributes, out NSError error);

		[Static, Export ("contentsOfDirectoryAtPath:error:")]
		NSObject [] ContentsOfDirectoryAtPath (string path, out NSError error);

		[Static, Export ("fileExistsAtPath:isDirectory:")]
		bool FileExistsAtPath (string path, bool isDirectory);

		[Static, Export ("moveItemAtPath:toPath:error:")]
		bool MoveItemAtPath (string srcPath, string dstPath, out NSError error);

		[Static, Export ("truncateFileAtPath:atOffset:error:")]
		bool TruncateFileAtPath (string filePath, ulong offset, out NSError error);

		[Static, Export ("getAbsoluteEncryptedPath:")]
		string GetAbsoluteEncryptedPath (string filePath);

		[Static, Export ("exportLogFileToDocumentsFolder:")]
		bool ExportLogFileToDocumentsFolder (out NSError error);

		[Static, Export ("uploadLogs:")]
		bool UploadLogs (out NSError error);
	}

	[BaseType (typeof (NSObject))]
	public interface GDAppEvent {

		[Export ("message", ArgumentSemantic.Copy)]
		string Message { get; set; }

		[Export ("code")]
		GDAppResultCode Code { get; set; }

		[Export ("type")]
		GDAppEventType Type { get; set; }
	}

	[BaseType (typeof (NSObject), Delegates = new string[] {"WeakDelegate"}, Events = new Type[] {typeof(GDiOSDelegate)})]
	public interface GDiOS {

		[Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set;  }
		
		[Wrap("WeakDelegate")]
		[NullAllowed]
		GDiOSDelegate Delegate { get; set; }

		//+ (void)initializeWithClassNameConformingToUIApplicationDelegate:(NSString*)applicationDelegate;
        [Static, Export ("initializeWithClassNameConformingToUIApplicationDelegate:")]
        void InitializeWithClassNameConformingToUIApplicationDelegate (string applicationDelegate);

		//+ (void)initialiseWithClassConformingToUIApplicationDelegate:(Class)applicationDelegate;
		[Static, Export ("initializeWithClassConformingToUIApplicationDelegate:")]
		void InitializeWithClassConformingToUIApplicationDelegate (Class applicationDelegate);
		
		//+ (BOOL)isInitialized;
		[Static, Export ("isInitialized")]
		bool IsInitialized();
		
		//+ (GDiOS*)sharedInstance;
		[Static, Export ("sharedInstance")]
		GDiOS SharedInstance();
		
		//- (UIWindow*)getWindow;
		[Export ("getWindow")]
		UIWindow GetWindow();

		//- (void)authorise:(NSString*)applicationId andVersion:(NSString*)version withDelegate:(id<GDiOSDelegate>)aDelegate;
		[Export("authorise:andVersion:withDelegate:")]
		void Authorize(string applicationId, string version, GDiOSDelegate aDelegate);

		[Export("authorize:")]
		void Authorize(GDiOSDelegate aDelegate);

		[Export("authorize")]
		void Authorize ();

		//- (NSDictionary*)getApplicationConfig
		[Export("getApplicationConfig")]
		NSDictionary GetApplicationConfig ();

		//- (BOOL) showPreferenceUI:(UIViewController *)baseViewController	
		[Export("showPreferenceUI:")]
		bool ShowPreferenceUI ([NullAllowed]UIViewController baseViewController);

		[Export("getServiceProvidersFor:andVersion:andType:")]
		NSArray GetServiceProviders (string serviceId, string version, GDServiceProviderType type);
	}

    [Protocol, BaseType(typeof(UIApplicationDelegate)), Model]
	public interface GDiOSDelegate {
		[Abstract, Export("handleEvent:")]
		void HandleEvent(GDAppEvent anEvent);
	}

    [Protocol, BaseType(typeof(NSObject)), Model]
	public interface GDSocketDelegate {

		[Export ("onOpen:")]
		void OnOpen(NSObject socket);

		[Export ("onRead:")]
		void  OnRead(NSObject socket);

		[Export ("onClose:")]
		void  OnClose(NSObject socket);

		[Export ("onErr:inSocket:")]
		void InSocket (int error, NSObject socket);
	}

	[BaseType (typeof (NSObject))]
	public interface GDDirectByteBuffer {
		[Export ("write:")]
		void Write (IntPtr data);

		[Export ("writeData:")]
		void WriteData (NSData data);

		[Export ("write:withLength:")]
		void Write (IntPtr data, int length);

		[Export ("bytesUnread")]
		int BytesUnread { get; }

		[Export ("read:toMaxLength:")]
        int Read (IntPtr data, int maxLength);

		[Export ("unreadDataAsString")]
		string UnreadDataAsString { get; }

		[Export ("unreadData")]
		NSMutableData UnreadData { get; }
	}

	[BaseType(typeof(NSObject))]
	public interface GDSocket {

		[Export ("init:onPort:andUseSSL:")]
		IntPtr Constructor (IntPtr url, int port, bool ssl);

		[Export ("disableHostVerification")]
		bool DisableHostVerification { get;}

		[Export ("disablePeerVerification")]
		bool DisablePeerVerification { get;}

		[Export ("connect")]
		void Connect ();

		[Export ("write")]
		void Write ();

		[Export ("disconnect")]
		void Disconnect ();

		[Export ("delegate", ArgumentSemantic.Assign)]
		GDSocketDelegate Delegate { get; set;}

		[Export ("writeStream", ArgumentSemantic.Retain)]
		GDDirectByteBuffer WriteStream { get; set;}

		[Export ("readStream", ArgumentSemantic.Retain)]
		GDDirectByteBuffer ReadStream { get; set; }
	}

    [Protocol, BaseType(typeof(NSObject)), Model]
	public interface GDHttpRequestDelegate {

		[Export ("onStatusChange:")]
        void  OnStatusChange(GDHttpRequest httpRequest);
	}
		
    [BaseType(typeof(NSObject), Delegates = new string[] {"WeakDelegate"}, Events = new Type[] {typeof(GDHttpRequestDelegate)})]
	public interface GDHttpRequest{

		[Export ("open:withUrl:withAsync:withUser:withPass:withAuth:")]
        bool Open ([PlainString] string method, [PlainString] string url, bool isAsync, [PlainString] string user, [PlainString] string password, [PlainString] string auth);

		[Export ("open:withUrl:withUser:withPass:withAuth:")]
        bool Open ([PlainString] string method, [PlainString] string url, [PlainString] string user, [PlainString] string password, [PlainString] string auth);

		[Export ("open:withUrl:withAsync:")]
        bool Open ([PlainString] string method, [PlainString] string url, bool isAsync);

		[Export ("open:withUrl:")]
        bool Open ([PlainString] string method, string url);

		[Export ("disableHostVerification")]
		bool DisableHostVerification {get;}

		[Export ("disablePeerVerification")]
		bool DisablePeerVerification { get;}

		[Export ("disableFollowLocation")]
		bool DisableFollowLocation { get; }

		[Export ("disableCookieHandling")]
		bool DisableCookieHandling { get; }

		[Export ("clearCookies:")]
		void ClearCookies (bool includePersistentStore);

		[Export ("enableHttpProxy:withPort:withUser:withPass:withAuth:")]
        bool EnableHttpProxy ([PlainString] string host, int port, [PlainString] string user, [PlainString] string password, [PlainString] string auth);

		[Export ("enableHttpProxy:withPort:")]
        bool EnableHttpProxy ([PlainString] string host, int port);

		[Export ("disableHttpProxy")]
		bool DisableHttpProxy { get; }

		[Export ("setRequestHeader:withValue:")]
        bool SetRequestHeader ([PlainString] string header, [PlainString] string value);

		[Export ("setPostValue:forKey:")]
        void SetPostValue ([PlainString] string value, [PlainString] string key);

		[Export ("clearPostValues")]
		void ClearPostValues ();

		[Export ("send:withLength:withTimeout:")]
		bool Send (IntPtr data, uint len, int timeout_s);

		[Export ("send:withTimeout:")]
		bool Send (IntPtr data, int timeout_s);

		[Export ("send:")]
		bool Send (IntPtr data);

		[Export ("send")]
        bool Send();

		[Export ("sendData:withTimeout:")]
		bool SendData (NSData data, int timeout_s);

		[Export ("sendData:")]
		bool SendData (NSData data);

		[Export ("sendWithFile:withTimeout:")]
		bool SendWithFile (string pathAndFileName, double timeoutSeconds);

		[Export ("sendWithFile:")]
		bool SendWithFile (string pathAndFileName);

		[Export ("getState")]
        GDHttpRequest_state_t GetState { get; }

		[Export ("getResponseHeader:")]
		IntPtr GetResponseHeader ([PlainString] string header);

		[Export ("getAllResponseHeaders")]
		IntPtr GetAllResponseHeaders { get;}

		[Export ("getStatus")]
		int GetStatus { get; }

		[Export ("getStatusText")]
		[PlainString] string GetStatusText { get; }

		[Export ("getReceiveBuffer")]
		GDDirectByteBuffer GetReceiveBuffer { get; }

		[Export ("close")]
		bool Close { get; }

		[Export ("abort")]
        bool Abort();

		[Export ("enablePipelining")]
		bool EnablePipelining { get; set;}

        [Wrap("WeakDelegate")]
		GDHttpRequestDelegate Delegate { get; set; }

        [Export ("delegate", ArgumentSemantic.Assign)]
        NSObject WeakDelegate { get; set;  }
	}

	[Category, BaseType (typeof (NSUrlCache))]
	public interface GDURLCache_NSURLCache {

		[Export ("maxCacheFileAge:")]
		void MaxCacheFileAge(double age);

		[Export ("maxCacheFileSize:")]
		void MaxCacheFileSize(int fileSize);
	}

	[BaseType (typeof (NSObject))]
	public interface GDURLLoadingSystem {

		[Static, Export ("enableSecureCommunication")]
		void EnableSecureCommunication ();

		[Static, Export ("disableSecureCommunication")]
		void DisableSecureCommunication ();
	}

	[BaseType (typeof (NSObject))]
	public interface GDCacheController {

		[Static, Export ("clearCredentialsForMethod:")]
		void ClearCredentialsForMethod (string method);

		[Static, Export ("kerberosAllowDelegation:")]
		void KerberosAllowDelegation (bool allow);
	}

	[Model, BaseType(typeof(NSObject))]
	public interface GDPushConnectionDelegate {

		[Export ("onStatus:")]
		void OnStatus (int status);
	}

	[BaseType(typeof(NSObject))]
	public interface GDPushConnection {

		[Static, Export ("sharedConnection")]
		NSObject SharedConnection { get; }

		[Export ("connect")]
		void Connect ();

		[Export ("disconnect")]
		void Disconnect ();

		[Export ("isConnected")]
		bool IsConnected { get; }

		[Export ("delegate", ArgumentSemantic.Assign)]
		GDPushConnectionDelegate Delegate { get; set; }
	}

    [Protocol, Model, BaseType(typeof(NSObject))]
	public interface GDPushChannelDelegate {

		[Export ("onChannelOpen:")]
		void OnChannelOpen (string token);

		[Export ("onChannelMessage:")]
		void OnChannelMessage (string data);

		[Export ("onChannelClose:")]
		void OnChannelClose (string data);

		[Export ("onChannelError:")]
		void OnChannelError (int error);

		[Export ("onChannelPingFail:")]
		void OnChannelPingFail (int error);
	}

	[BaseType(typeof(NSObject))]
	public interface GDPushChannel {


		[Export ("connect")]
		void Connect ();

		[Export ("disconnect")]
		void Disconnect ();

		[Export ("delegate", ArgumentSemantic.Assign)]
		GDPushChannelDelegate Delegate { get; set; }

		[Field ("kGDSecureDocsScheme", "__Internal")]
		NSString GDSecureDocsScheme { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface GDSecureDocs {

		[Static, Export ("canSendFileToGFE")]
		bool CanSendFileToGFE { get; }

		[Static, Export ("canSendFileToApplication:")]
		bool CanSendFileToApplication (string application);

		[Static, Export ("sendFileToGFE:withSuccessBlock:")]
		bool SendFileToGFE (string relativeSecureFile, SendFileSuccessBlock block);

		[Static, Export ("sendFile:toApplication:withSuccessBlock:")]
		bool SendFile (string relativeSecureFile, string application, SendFileSuccessBlock block);
	}

    [Protocol, Model, BaseType(typeof(NSObject))]
	public interface GDServiceClientDelegate {

		[Export ("GDServiceClientDidReceiveFrom:withParams:withAttachments:correspondingToRequestID:")]
		void DidRecieveFrom (string application, NSObject parameters, NSObject [] attachments, string requestID);

		[Export ("GDServiceClientDidFinishSendingTo:withAttachments:withParams:correspondingToRequestID:")]
		void DidFinishSendingTo (string application, NSObject [] attachments, NSObject parameters, string requestID);
	}

	[BaseType(typeof(NSObject))]
	public interface GDServiceClient {

		[Static, Export ("sendTo:withService:withVersion:withMethod:withParams:withAttachments:bringServiceToFront:requestID:error:")]
		bool SendTo ([NullAllowed] string application, string service, string version, string method, [NullAllowed] NSObject parameters, [NullAllowed] NSObject[] attachments, GDTForegroundOption option, out string requestID, out NSError error);

		[Static, Export ("bringToFront:error:")]
		bool BringToFront (string application, out NSError error);

		[Export ("delegate", ArgumentSemantic.Assign)]
		GDServiceClientDelegate Delegate { [Bind ("getDelegate")] get; set; }
	}

    [Protocol, Model, BaseType(typeof(NSObject))]
	public interface GDServiceDelegate {

		[Export ("GDServiceDidReceiveFrom:forService:withVersion:forMethod:withParams:withAttachments:forRequestID:")]
		void ForService (string application, string service, string version, string method, NSObject parameters, NSObject [] attachments, string requestID);

		[Export ("GDServiceDidFinishSendingTo:withAttachments:withParams:correspondingToRequestID:")]
		void WithAttachments (string application, NSObject [] attachments, NSObject parameters, string requestID);
	}

	[BaseType(typeof(NSObject))]
	public interface GDService {

		[Static, Export ("replyTo:withParams:bringClientToFront:withAttachments:requestID:error:")]
		bool ReplyTo (string application, [NullAllowed] NSObject parameters, GDTForegroundOption option, [NullAllowed] NSObject[] attachments, string requestID, out NSError error);

		[Static, Export ("bringToFront:error:")]
		bool BringToFront (string application, out NSError error);

		[Export ("delegate", ArgumentSemantic.Assign)]
		GDServiceDelegate Delegate { [Bind ("getDelegate")] get; set; }

		[Field("MTGDFrontRequestService", "__Internal")]
		NSString GDFrontRequestService {get;}

		[Field("MTGDFrontRequestMethod", "__Internal")]
		NSString GDFrontRequestMethod {get;}

		[Field ("MTGDServicesErrorDomain", "__Internal")]
		NSString GDServicesErrorDomain { get; }

		[Field ("MTGDServicesErrorGeneral", "__Internal")]
		int GDServicesErrorGeneral { get; }

		[Field ("MTGDServicesErrorApplicationNotFound", "__Internal")]
		int GDServicesErrorApplicationNotFound { get; }

		[Field ("MTGDServicesErrorServiceNotFound", "__Internal")]
		int GDServicesErrorServiceNotFound { get; }

		[Field ("MTGDServicesErrorServiceVersionNotFound", "__Internal")]
		int GDServicesErrorServiceVersionNotFound { get; }

		[Field ("MTGDServicesErrorMethodNotFound", "__Internal")]
		int GDServicesErrorMethodNotFound { get; }

		[Field ("MTGDServicesErrorNotAllowed", "__Internal")]
		int GDServicesErrorNotAllowed { get; }

		[Field ("MTGDServicesErrorInvalidParams", "__Internal")]
		int GDServicesErrorInvalidParams { get; }

		[Field ("MTGDServicesErrorCertificateNotFound", "__Internal")]
		int GDServicesErrorCertificateNotFound { get; }

		[Field ("MTGDServicesMethodDisabled", "__Internal")]
		int GDServicesMethodDisabled { get; }

		[Field ("MTGDServicesVersionDisabled", "__Internal")]
		int GDServicesVersionDisabled { get; }

		[Field ("MTGDServicesServiceDisabled", "__Internal")]
		int GDServicesServiceDisabled { get; }
	}

    [Protocol, Model, BaseType(typeof(NSObject))]
	public interface GDAuthTokenDelegate {

		[Export ("onGDAuthTokenSuccess:")]
		void OnGDAuthTokenSuccess (string gdAuthToken);

		[Export ("onGDAuthTokenFailure:")]
		void OnGDAuthTokenFailure (NSError authTokenError);
	}

	[BaseType(typeof(NSObject))]
	public interface GDUtility{

		[Export ("getGDAuthToken:")]
		void GetGDAuthToken (string challenge);

		[Export ("gdAuthDelegate", ArgumentSemantic.Assign)]
		GDAuthTokenDelegate GdAuthDelegate { [Bind ("getGDAuthDelegate")] get; [Bind ("setGDAuthDelegate:")] set;}

		[Field ("GDAuthTokenDomain", "__Internal")]
		NSString GDAuthTokenDomain { get;}
	}

	[Category, BaseType (typeof (NSMutableUrlRequest))]
	public partial interface GDNET_NSMutableURLRequest {

		[Export ("failOnAuthorizationChallenge")]
		bool FailOnAuthorizationChallenge();

		[Export ("setAuthorizationCredentials:withProtectionSpace:")]
		bool SetAuthorizationCredentials (NSUrlCredential credentials, NSUrlProtectionSpace space);

		[Export ("disableHostVerification")]
		bool DisableHostVerification();

		[Export ("disablePeerVerification")]
		bool DisablePeerVerification();
	}
}
