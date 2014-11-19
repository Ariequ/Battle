/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace Hero.Config
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class FuncData : TBase
  {
    private int _id;
    private sbyte _oper;
    private sbyte _target;
    private List<string> _paramStringList;
    private List<int> _paramIntList;

    public int Id
    {
      get
      {
        return _id;
      }
      set
      {
        __isset.id = true;
        this._id = value;
      }
    }

    public sbyte Oper
    {
      get
      {
        return _oper;
      }
      set
      {
        __isset.oper = true;
        this._oper = value;
      }
    }

    public sbyte Target
    {
      get
      {
        return _target;
      }
      set
      {
        __isset.target = true;
        this._target = value;
      }
    }

    public List<string> ParamStringList
    {
      get
      {
        return _paramStringList;
      }
      set
      {
        __isset.paramStringList = true;
        this._paramStringList = value;
      }
    }

    public List<int> ParamIntList
    {
      get
      {
        return _paramIntList;
      }
      set
      {
        __isset.paramIntList = true;
        this._paramIntList = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool id;
      public bool oper;
      public bool target;
      public bool paramStringList;
      public bool paramIntList;
    }

    public FuncData() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I32) {
              Id = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Byte) {
              Oper = iprot.ReadByte();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Byte) {
              Target = iprot.ReadByte();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.List) {
              {
                ParamStringList = new List<string>();
                TList _list50 = iprot.ReadListBegin();
                for( int _i51 = 0; _i51 < _list50.Count; ++_i51)
                {
                  string _elem52 = null;
                  _elem52 = iprot.ReadString();
                  ParamStringList.Add(_elem52);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.List) {
              {
                ParamIntList = new List<int>();
                TList _list53 = iprot.ReadListBegin();
                for( int _i54 = 0; _i54 < _list53.Count; ++_i54)
                {
                  int _elem55 = 0;
                  _elem55 = iprot.ReadI32();
                  ParamIntList.Add(_elem55);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("FuncData");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.id) {
        field.Name = "id";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Id);
        oprot.WriteFieldEnd();
      }
      if (__isset.oper) {
        field.Name = "oper";
        field.Type = TType.Byte;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteByte(Oper);
        oprot.WriteFieldEnd();
      }
      if (__isset.target) {
        field.Name = "target";
        field.Type = TType.Byte;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteByte(Target);
        oprot.WriteFieldEnd();
      }
      if (ParamStringList != null && __isset.paramStringList) {
        field.Name = "paramStringList";
        field.Type = TType.List;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.String, ParamStringList.Count));
          foreach (string _iter56 in ParamStringList)
          {
            oprot.WriteString(_iter56);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (ParamIntList != null && __isset.paramIntList) {
        field.Name = "paramIntList";
        field.Type = TType.List;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.I32, ParamIntList.Count));
          foreach (int _iter57 in ParamIntList)
          {
            oprot.WriteI32(_iter57);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("FuncData(");
      sb.Append("Id: ");
      sb.Append(Id);
      sb.Append(",Oper: ");
      sb.Append(Oper);
      sb.Append(",Target: ");
      sb.Append(Target);
      sb.Append(",ParamStringList: ");
      sb.Append(ParamStringList);
      sb.Append(",ParamIntList: ");
      sb.Append(ParamIntList);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
