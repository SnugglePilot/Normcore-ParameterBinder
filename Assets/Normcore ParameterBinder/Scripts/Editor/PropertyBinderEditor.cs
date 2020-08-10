#region License
//------------------------------------------------------------------------------ -
// Normcore-ParameterBinder
// https://github.com/chetu3319/Normcore-ParameterBinder
//------------------------------------------------------------------------------ -
// Original Author: Keijiro Takahashi
// Gituhb Repo: https://github.com/keijiro/Lasp/blob/v2/Packages/jp.keijiro.lasp/Editor/PropertyBinderEditor.cs
//------------------------------------------------------------------------------ -
//
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


using System;
using UnityEngine;
using UnityEditor;


//
// Custom editor for editing property binder list
//
sealed class PropertyBinderEditor
{
    #region Public methods

    public PropertyBinderEditor(SerializedProperty binders,string datatype)
    {
        _binders = binders;
        _datatype = datatype; 
        ComponentSelector.InvalidateCache();
    }

    public void ShowGUI()
    {
        EditorGUILayout.Space();

        for (var i = 0; i < _binders.arraySize; i++)
        {
            Action<int> showPropertyBinderEditor = ShowPropertyBinderEditor;
            showPropertyBinderEditor(i);
        }

        CoreEditorUtils.DrawSplitter();
        EditorGUILayout.Space();

        // "Add Property Binder" button
        var rect = EditorGUILayout.GetControlRect();
        rect.x += (rect.width - 200) / 2;
        rect.width = 200;

        if (GUI.Button(rect, "Add " + _datatype + "Property Binder"))
        {
           //  CreateNewPropertyBinderMenu(_datatype).DropDown(rect);
             //OnAddNewPropertyBinder<FloatValuePropertyBinder>();
             //CreateNewProprtyBinder(_datatype);
             switch (_datatype)        
             {
                 case "bool": 
                     OnAddNewPropertyBinder<BoolValuePropertyBinder>();
                     break;
                 case "float":
                     CreateFloatPropertyBinderMenu().DropDown(rect);
                     break;
                 case "Vector3":
                     OnAddNewPropertyBinder<Vector3ValuePropertyBinder>();
                     break;
             }
            
        }

       
            
    }

    #endregion

    #region Private members

    SerializedProperty _binders;
    private String _datatype; 

    static class Styles
    {
        // public static Label Value0 = "Value at 0";
        // public static Label Value1 = "Value at 1";
        public static Label MoveUp = "Move Up";
        public static Label MoveDown = "Move Down";
        public static Label Remove = "Remove";
    }

    #endregion

    #region "Add Property Binder" button

    GenericMenu CreateNewPropertyBinderMenu(string dataType)
    {
        var menu = new GenericMenu();
        if (dataType == "bool")
        {
            AddNewPropertyBinderItem<BoolValuePropertyBinder>(menu);   
        }

        if (dataType == "float")
        {
            AddNewPropertyBinderItem<FloatValuePropertyBinder>(menu);
        }

        // AddNewPropertyBinderItem<FloatPropertyBinder>(menu);
        // AddNewPropertyBinderItem<Vector3PropertyBinder>(menu);
        // AddNewPropertyBinderItem<EulerRotationPropertyBinder>(menu);
        // AddNewPropertyBinderItem<ColorPropertyBinder>(menu);
       
        return menu;
    }
    
    GenericMenu CreateFloatPropertyBinderMenu()
    {
        var menu = new GenericMenu();
        

        AddNewPropertyBinderItem<FloatValuePropertyBinder>(menu);
        AddNewPropertyBinderItem<Vector3ValuePropertyBinderFloat>(menu);
        
       
        return menu;
    }

    public void AddNewPropertyBinderItem<T>(GenericMenu menu) where T : new()
      => menu.AddItem(PropertyBinderTypeLabel<T>.Content,
                      false, OnAddNewPropertyBinder<T>);

    void CreateNewProprtyBinder(string dataType)
    {
        // if (dataType == "bool")
        // {
        //     OnAddNewPropertyBinder<BoolValuePropertyBinder>();   
        // }
        //
        // if (dataType == "float")
        // {
        //     OnAddNewPropertyBinder<FloatValuePropertyBinder>();
        // }

        switch (dataType)        
        {
            case "bool": 
                OnAddNewPropertyBinder<BoolValuePropertyBinder>();
                break;
            case "float":
                var menu = new GenericMenu();
                
                OnAddNewPropertyBinder<FloatValuePropertyBinder>();
                break;
            case "Vector3":
                OnAddNewPropertyBinder<Vector3ValuePropertyBinder>();
                break;
        }
    }

    void OnAddNewPropertyBinder<T>() where T : new()
    {
        _binders.serializedObject.Update();

        var i = _binders.arraySize;
        _binders.InsertArrayElementAtIndex(i);
        _binders.GetArrayElementAtIndex(i).managedReferenceValue = new T();

        _binders.serializedObject.ApplyModifiedProperties();
    }

    #endregion

    #region PropertyBinder editor

    void ShowPropertyBinderEditor(int index)
    {
        var prop = _binders.GetArrayElementAtIndex(index);
        var finder = new RelativePropertyFinder(prop);

        // Header
        CoreEditorUtils.DrawSplitter();

        var toggle = CoreEditorUtils.DrawHeaderToggle
          (PropertyBinderNameUtil.Shorten(prop),
           prop, finder["Enabled"],
           pos => CreateHeaderContextMenu(index)
                  .DropDown(new Rect(pos, Vector2.zero)));

        if (!toggle) return;

        _binders.serializedObject.Update();
        EditorGUILayout.Space();

        // Properties
        var target = finder["Target"];
        EditorGUILayout.PropertyField(target);

        if (ComponentSelector.GetInstance(target).ShowGUI(target) &&
            PropertySelector.GetInstance(target, finder["_propertyType"])
            .ShowGUI(finder["PropertyName"]))
        {
            // EditorGUILayout.PropertyField(finder["Value0"], Styles.Value0);
            // EditorGUILayout.PropertyField(finder["Value1"], Styles.Value1);
        }

        _binders.serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();
    }

    #endregion

    #region ProeprtyBinder editor context menu

    GenericMenu CreateHeaderContextMenu(int index)
    {
        var menu = new GenericMenu();

        // Move up
        if (index == 0)
            menu.AddDisabledItem(Styles.MoveUp);
        else
            menu.AddItem(Styles.MoveUp, false,
                         () => OnMoveControl(index, index - 1));

        // Move down
        if (index == _binders.arraySize - 1)
            menu.AddDisabledItem(Styles.MoveDown);
        else
            menu.AddItem(Styles.MoveDown, false,
                         () => OnMoveControl(index, index + 1));

        menu.AddSeparator(string.Empty);

        // Remove
        menu.AddItem(Styles.Remove, false, () => OnRemoveControl(index));

        return menu;
    }

    void OnMoveControl(int src, int dst)
    {
        _binders.serializedObject.Update();
        _binders.MoveArrayElement(src, dst);
        _binders.serializedObject.ApplyModifiedProperties();
    }

    void OnRemoveControl(int index)
    {
        _binders.serializedObject.Update();
        _binders.DeleteArrayElementAtIndex(index);
        _binders.serializedObject.ApplyModifiedProperties();
    }

    #endregion
}

