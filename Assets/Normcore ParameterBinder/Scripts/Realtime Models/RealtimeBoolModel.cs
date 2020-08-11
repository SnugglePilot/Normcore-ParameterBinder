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
using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class RealtimeBoolModel
{
    [RealtimeProperty(1,true,true)]
    public bool _boolProperty;
}


/* ----- Begin Normal Autogenerated Code ----- */
public partial class RealtimeBoolModel : RealtimeModel {
    public bool boolProperty {
        get {
            return _cache.LookForValueInCache(_boolProperty, entry => entry.boolPropertySet, entry => entry.boolProperty);
        }
        set {
            if (boolProperty == value) return;
            _cache.UpdateLocalCache(entry => { entry.boolPropertySet = true; entry.boolProperty = value; return entry; });
            InvalidateReliableLength();
            FireBoolPropertyDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(RealtimeBoolModel model, T value);
    public event PropertyChangedHandler<bool> boolPropertyDidChange;
    
    private struct LocalCacheEntry {
        public bool boolPropertySet;
        public bool boolProperty;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache = new LocalChangeCache<LocalCacheEntry>();
    
    public enum PropertyID : uint {
        BoolProperty = 1,
    }
    
    public RealtimeBoolModel() : this(null) {
    }
    
    public RealtimeBoolModel(RealtimeModel parent) : base(null, parent) {
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        UnsubscribeClearCacheCallback();
    }
    
    private void FireBoolPropertyDidChange(bool value) {
        try {
            boolPropertyDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        int length = 0;
        if (context.fullModel) {
            FlattenCache();
            length += WriteStream.WriteVarint32Length((uint)PropertyID.BoolProperty, _boolProperty ? 1u : 0u);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.boolPropertySet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.BoolProperty, entry.boolProperty ? 1u : 0u);
            }
        }
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var didWriteProperties = false;
        
        if (context.fullModel) {
            stream.WriteVarint32((uint)PropertyID.BoolProperty, _boolProperty ? 1u : 0u);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.boolPropertySet) {
                _cache.PushLocalCacheToInflight(context.updateID);
                ClearCacheOnStreamCallback(context);
            }
            if (entry.boolPropertySet) {
                stream.WriteVarint32((uint)PropertyID.BoolProperty, entry.boolProperty ? 1u : 0u);
                didWriteProperties = true;
            }
            
            if (didWriteProperties) InvalidateReliableLength();
        }
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.BoolProperty: {
                    bool previousValue = _boolProperty;
                    _boolProperty = (stream.ReadVarint32() != 0);
                    bool boolPropertyExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.boolPropertySet);
                    if (!boolPropertyExistsInChangeCache && _boolProperty != previousValue) {
                        FireBoolPropertyDidChange(_boolProperty);
                    }
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
        }
    }
    
    #region Cache Operations
    
    private StreamEventDispatcher _streamEventDispatcher;
    
    private void FlattenCache() {
        _boolProperty = boolProperty;
        _cache.Clear();
    }
    
    private void ClearCache(uint updateID) {
        _cache.RemoveUpdateFromInflight(updateID);
    }
    
    private void ClearCacheOnStreamCallback(StreamContext context) {
        if (_streamEventDispatcher != context.dispatcher) {
            UnsubscribeClearCacheCallback(); // unsub from previous dispatcher
        }
        _streamEventDispatcher = context.dispatcher;
        _streamEventDispatcher.AddStreamCallback(context.updateID, ClearCache);
    }
    
    private void UnsubscribeClearCacheCallback() {
        if (_streamEventDispatcher != null) {
            _streamEventDispatcher.RemoveStreamCallback(ClearCache);
            _streamEventDispatcher = null;
        }
    }
    
    #endregion
}
/* ----- End Normal Autogenerated Code ----- */
