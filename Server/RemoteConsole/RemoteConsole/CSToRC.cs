//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: CSToRC.proto
namespace CSToRC
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"QueryCDkeyInfoResp")]
  public partial class QueryCDkeyInfoResp : global::ProtoBuf.IExtensible
  {
    public QueryCDkeyInfoResp() {}
    
    private CSToRC.MsgID _msgid = CSToRC.MsgID.eMsgCS2RC_AskQueryCDkeyResp;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"msgid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(CSToRC.MsgID.eMsgCS2RC_AskQueryCDkeyResp)]
    public CSToRC.MsgID msgid
    {
      get { return _msgid; }
      set { _msgid = value; }
    }
    private readonly global::System.Collections.Generic.List<CSToRC.QueryCDkeyInfoResp.CDKey_Info> _info = new global::System.Collections.Generic.List<CSToRC.QueryCDkeyInfoResp.CDKey_Info>();
    [global::ProtoBuf.ProtoMember(2, Name=@"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<CSToRC.QueryCDkeyInfoResp.CDKey_Info> info
    {
      get { return _info; }
    }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CDKey_Info")]
  public partial class CDKey_Info : global::ProtoBuf.IExtensible
  {
    public CDKey_Info() {}
    
    private int _id = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private string _title = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"title", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string title
    {
      get { return _title; }
      set { _title = value; }
    }
    private int _platform = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"platform", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int platform
    {
      get { return _platform; }
      set { _platform = value; }
    }
    private long _end_tiem = default(long);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"end_tiem", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long end_tiem
    {
      get { return _end_tiem; }
      set { _end_tiem = value; }
    }
    private int _code_num = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"code_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int code_num
    {
      get { return _code_num; }
      set { _code_num = value; }
    }
    private int _code_len = default(int);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"code_len", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int code_len
    {
      get { return _code_len; }
      set { _code_len = value; }
    }
    private int _type = default(int);
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int type
    {
      get { return _type; }
      set { _type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AddMailResp")]
  public partial class AddMailResp : global::ProtoBuf.IExtensible
  {
    public AddMailResp() {}
    
    private CSToRC.MsgID _msgid = CSToRC.MsgID.eMsgCS2RC_AskAddOneMailResp;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"msgid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(CSToRC.MsgID.eMsgCS2RC_AskAddOneMailResp)]
    public CSToRC.MsgID msgid
    {
      get { return _msgid; }
      set { _msgid = value; }
    }
    private bool _rst = default(bool);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"rst", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(default(bool))]
    public bool rst
    {
      get { return _rst; }
      set { _rst = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AskRegistRsp")]
  public partial class AskRegistRsp : global::ProtoBuf.IExtensible
  {
    public AskRegistRsp() {}
    
    private CSToRC.MsgID _msgid = CSToRC.MsgID.eMsgCS2RC_AskRegistRsp;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"msgid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(CSToRC.MsgID.eMsgCS2RC_AskRegistRsp)]
    public CSToRC.MsgID msgid
    {
      get { return _msgid; }
      set { _msgid = value; }
    }
    private bool _rst = default(bool);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"rst", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(default(bool))]
    public bool rst
    {
      get { return _rst; }
      set { _rst = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AllServerInfo")]
  public partial class AllServerInfo : global::ProtoBuf.IExtensible
  {
    public AllServerInfo() {}
    
    private CSToRC.MsgID _msgid = CSToRC.MsgID.eMsgCS2RC_AllServerInfo;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"msgid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(CSToRC.MsgID.eMsgCS2RC_AllServerInfo)]
    public CSToRC.MsgID msgid
    {
      get { return _msgid; }
      set { _msgid = value; }
    }
    private byte[] _servername = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"servername", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public byte[] servername
    {
      get { return _servername; }
      set { _servername = value; }
    }
    private int _roomUserNum = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"roomUserNum", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int roomUserNum
    {
      get { return _roomUserNum; }
      set { _roomUserNum = value; }
    }
    private int _battleUserNum = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"battleUserNum", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int battleUserNum
    {
      get { return _battleUserNum; }
      set { _battleUserNum = value; }
    }
    private float _loadFactory = default(float);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"loadFactory", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    [global::System.ComponentModel.DefaultValue(default(float))]
    public float loadFactory
    {
      get { return _loadFactory; }
      set { _loadFactory = value; }
    }
    private int _allUserNum = default(int);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"allUserNum", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int allUserNum
    {
      get { return _allUserNum; }
      set { _allUserNum = value; }
    }
    private string _curttime = "";
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"curttime", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string curttime
    {
      get { return _curttime; }
      set { _curttime = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ServerUserInfo")]
  public partial class ServerUserInfo : global::ProtoBuf.IExtensible
  {
    public ServerUserInfo() {}
    
    private CSToRC.MsgID _msgid = CSToRC.MsgID.eMsgCS2RC_ServerUserInfo;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"msgid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(CSToRC.MsgID.eMsgCS2RC_ServerUserInfo)]
    public CSToRC.MsgID msgid
    {
      get { return _msgid; }
      set { _msgid = value; }
    }
    private int _allUserNum = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"allUserNum", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int allUserNum
    {
      get { return _allUserNum; }
      set { _allUserNum = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"NoticeInfo")]
  public partial class NoticeInfo : global::ProtoBuf.IExtensible
  {
    public NoticeInfo() {}
    
    private byte[] _notice = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"notice", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public byte[] notice
    {
      get { return _notice; }
      set { _notice = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AllNoticeInfo")]
  public partial class AllNoticeInfo : global::ProtoBuf.IExtensible
  {
    public AllNoticeInfo() {}
    
    private CSToRC.MsgID _msgid = CSToRC.MsgID.eMsgCS2RC_AllNoticeInfo;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"msgid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(CSToRC.MsgID.eMsgCS2RC_AllNoticeInfo)]
    public CSToRC.MsgID msgid
    {
      get { return _msgid; }
      set { _msgid = value; }
    }
    private readonly global::System.Collections.Generic.List<CSToRC.NoticeInfo> _notices = new global::System.Collections.Generic.List<CSToRC.NoticeInfo>();
    [global::ProtoBuf.ProtoMember(2, Name=@"notices", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<CSToRC.NoticeInfo> notices
    {
      get { return _notices; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AddNoticeResp")]
  public partial class AddNoticeResp : global::ProtoBuf.IExtensible
  {
    public AddNoticeResp() {}
    
    private CSToRC.MsgID _msgid = CSToRC.MsgID.eMsgCS2RC_AddNoticeResp;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"msgid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(CSToRC.MsgID.eMsgCS2RC_AddNoticeResp)]
    public CSToRC.MsgID msgid
    {
      get { return _msgid; }
      set { _msgid = value; }
    }
    private bool _rst = default(bool);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"rst", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(default(bool))]
    public bool rst
    {
      get { return _rst; }
      set { _rst = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"DeleteNoticeResp")]
  public partial class DeleteNoticeResp : global::ProtoBuf.IExtensible
  {
    public DeleteNoticeResp() {}
    
    private CSToRC.MsgID _msgid = CSToRC.MsgID.eMsgCS2RC_DeleteNoticeResp;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"msgid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(CSToRC.MsgID.eMsgCS2RC_DeleteNoticeResp)]
    public CSToRC.MsgID msgid
    {
      get { return _msgid; }
      set { _msgid = value; }
    }
    private bool _rst = default(bool);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"rst", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(default(bool))]
    public bool rst
    {
      get { return _rst; }
      set { _rst = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"MsgID")]
    public enum MsgID
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgCS2RC_Begin", Value=33500)]
      eMsgCS2RC_Begin = 33500,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgCS2RC_AskRegistRsp", Value=33501)]
      eMsgCS2RC_AskRegistRsp = 33501,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgCS2RC_AllServerInfo", Value=33502)]
      eMsgCS2RC_AllServerInfo = 33502,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgCS2RC_ServerUserInfo", Value=33503)]
      eMsgCS2RC_ServerUserInfo = 33503,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgCS2RC_AllNoticeInfo", Value=33504)]
      eMsgCS2RC_AllNoticeInfo = 33504,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgCS2RC_AddNoticeResp", Value=33505)]
      eMsgCS2RC_AddNoticeResp = 33505,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgCS2RC_DeleteNoticeResp", Value=33506)]
      eMsgCS2RC_DeleteNoticeResp = 33506,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgCS2RC_AskAddOneMailResp", Value=33507)]
      eMsgCS2RC_AskAddOneMailResp = 33507,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgCS2RC_AskQueryCDkeyResp", Value=33508)]
      eMsgCS2RC_AskQueryCDkeyResp = 33508,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eMsgCS2RC_End", Value=33999)]
      eMsgCS2RC_End = 33999
    }
  
}