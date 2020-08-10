#region License
//------------------------------------------------------------------------------ -
// Normcore-ParameterBinder
// https://github.com/chetu3319/Normcore-ParameterBinder
//------------------------------------------------------------------------------ -
// MIT License
//
// Copyright (c) 2020 Chaitanya Shah
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//------------------------------------------------------------------------------ -
#endregion
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class NormalPrimaryDatatypeModel
{
  [RealtimeProperty(1, true, true)] public bool _boolProperty;
  [RealtimeProperty(2, false, true)] public float _floatProperty;
  [RealtimeProperty(3, true, true)] public Vector3 _vector3Property; 

}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class NormalPrimaryDatatypeModel : IModel {
    // Properties
    public bool boolProperty {
        get { return _cache.LookForValueInCache(_boolProperty, entry => entry.boolPropertySet, entry => entry.boolProperty); }
        set { if (value == boolProperty) return; _cache.UpdateLocalCache(entry => { entry.boolPropertySet = true; entry.boolProperty = value; return entry; }); FireBoolPropertyDidChange(value); }
    }
    public float floatProperty {
        get { return _floatProperty; }
        set { if (value == _floatProperty) return; _floatPropertyShouldWrite = true; _floatProperty = value; FireFloatPropertyDidChange(value); }
    }
    public UnityEngine.Vector3 vector3Property {
        get { return _cache.LookForValueInCache(_vector3Property, entry => entry.vector3PropertySet, entry => entry.vector3Property); }
        set { if (value == vector3Property) return; _cache.UpdateLocalCache(entry => { entry.vector3PropertySet = true; entry.vector3Property = value; return entry; }); FireVector3PropertyDidChange(value); }
    }
    
    // Events
    public delegate void BoolPropertyDidChange(NormalPrimaryDatatypeModel model, bool value);
    public event         BoolPropertyDidChange boolPropertyDidChange;
    public delegate void FloatPropertyDidChange(NormalPrimaryDatatypeModel model, float value);
    public event         FloatPropertyDidChange floatPropertyDidChange;
    public delegate void Vector3PropertyDidChange(NormalPrimaryDatatypeModel model, UnityEngine.Vector3 value);
    public event         Vector3PropertyDidChange vector3PropertyDidChange;
    
    // Delta updates
    private struct LocalCacheEntry {
        public bool                boolPropertySet;
        public bool                boolProperty;
        public bool                vector3PropertySet;
        public UnityEngine.Vector3 vector3Property;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache;
    
    private bool _floatPropertyShouldWrite;
    
    public NormalPrimaryDatatypeModel() {
        _cache = new LocalChangeCache<LocalCacheEntry>();
    }
    
    // Events
    public void FireBoolPropertyDidChange(bool value) {
        try {
            if (boolPropertyDidChange != null)
                boolPropertyDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    public void FireFloatPropertyDidChange(float value) {
        try {
            if (floatPropertyDidChange != null)
                floatPropertyDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    public void FireVector3PropertyDidChange(UnityEngine.Vector3 value) {
        try {
            if (vector3PropertyDidChange != null)
                vector3PropertyDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    
    // Serialization
    enum PropertyID {
        BoolProperty = 1,
        FloatProperty = 2,
        Vector3Property = 3,
    }
    
    public int WriteLength(StreamContext context) {
        int length = 0;
        
        if (context.fullModel) {
            // Mark unreliable properties as clean and flatten the in-flight cache.
            // TODO: Move this out of WriteLength() once we have a prepareToWrite method.
            _floatPropertyShouldWrite = false;
            _boolProperty = boolProperty;
            _vector3Property = vector3Property;
            _cache.Clear();
            
            // Write all properties
            length += WriteStream.WriteVarint32Length((uint)PropertyID.BoolProperty, _boolProperty ? 1u : 0u);
            length += WriteStream.WriteFloatLength((uint)PropertyID.FloatProperty);
            length += WriteStream.WriteBytesLength((uint)PropertyID.Vector3Property, WriteStream.Vector3ToBytesLength());
        } else {
            // Unreliable properties
            if (context.unreliableChannel) {
                if (_floatPropertyShouldWrite) {
                    length += WriteStream.WriteFloatLength((uint)PropertyID.FloatProperty);
                }
            }
            
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.boolPropertySet)
                    length += WriteStream.WriteVarint32Length((uint)PropertyID.BoolProperty, entry.boolProperty ? 1u : 0u);
                if (entry.vector3PropertySet)
                    length += WriteStream.WriteBytesLength((uint)PropertyID.Vector3Property, WriteStream.Vector3ToBytesLength());
            }
        }
        
        return length;
    }
    
    public void Write(WriteStream stream, StreamContext context) {
        if (context.fullModel) {
            // Write all properties
            stream.WriteVarint32((uint)PropertyID.BoolProperty, _boolProperty ? 1u : 0u);
            stream.WriteFloat((uint)PropertyID.FloatProperty, _floatProperty);
            _floatPropertyShouldWrite = false;
            stream.WriteBytes((uint)PropertyID.Vector3Property, WriteStream.Vector3ToBytes(_vector3Property));
        } else {
            // Unreliable properties
            if (context.unreliableChannel) {
                if (_floatPropertyShouldWrite) {
                    stream.WriteFloat((uint)PropertyID.FloatProperty, _floatProperty);
                    _floatPropertyShouldWrite = false;
                }
            }
            
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.boolPropertySet || entry.vector3PropertySet)
                    _cache.PushLocalCacheToInflight(context.updateID);
                
                if (entry.boolPropertySet)
                    stream.WriteVarint32((uint)PropertyID.BoolProperty, entry.boolProperty ? 1u : 0u);
                if (entry.vector3PropertySet)
                    stream.WriteBytes((uint)PropertyID.Vector3Property, WriteStream.Vector3ToBytes(entry.vector3Property));
            }
        }
    }
    
    public void Read(ReadStream stream, StreamContext context) {
        bool boolPropertyExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.boolPropertySet);
        bool vector3PropertyExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.vector3PropertySet);
        
        // Remove from in-flight
        if (context.deltaUpdatesOnly && context.reliableChannel)
            _cache.RemoveUpdateFromInflight(context.updateID);
        
        // Loop through each property and deserialize
        uint propertyID;
        while (stream.ReadNextPropertyID(out propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.BoolProperty: {
                    bool previousValue = _boolProperty;
                    
                    _boolProperty = (stream.ReadVarint32() != 0);
                    
                    if (!boolPropertyExistsInChangeCache && _boolProperty != previousValue)
                        FireBoolPropertyDidChange(_boolProperty);
                    break;
                }
                case (uint)PropertyID.FloatProperty: {
                    float previousValue = _floatProperty;
                    
                    _floatProperty = stream.ReadFloat();
                    _floatPropertyShouldWrite = false;
                    
                    if (_floatProperty != previousValue)
                        FireFloatPropertyDidChange(_floatProperty);
                    break;
                }
                case (uint)PropertyID.Vector3Property: {
                    UnityEngine.Vector3 previousValue = _vector3Property;
                    
                    _vector3Property = ReadStream.Vector3FromBytes(stream.ReadBytes());
                    
                    if (!vector3PropertyExistsInChangeCache && _vector3Property != previousValue)
                        FireVector3PropertyDidChange(_vector3Property);
                    break;
                }
                default:
                    stream.SkipProperty();
                    break;
            }
        }
    }
}
/* ----- End Normal Autogenerated Code ----- */
