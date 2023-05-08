using System;
using Core.Datas;
using Core.Technologies;
using Editor.CustomTreeEditor.Connections;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomTreeEditor
{
    public class TechnologiesTreeEditorNode
    {
        public string Title { get; private set; }
        public Rect Rect => _rect;
        public Technology DataNode { get; }
        public ConnectionPoint InPoint { get => _inPoint; }
        public ConnectionPoint OutPoint { get => _outPoint; }

        private bool _isSelected;
        private bool _isDragged;
        private Rect _rect;
        private GUIStyle _style;
        private GUIStyle _defaultNodeStyle;
        private GUIStyle _selectedNodeStyle;
        private ConnectionPoint _inPoint;
        private ConnectionPoint _outPoint;
        private Action<TechnologiesTreeEditorNode> _onSelectNode;
        private Action<TechnologiesTreeEditorNode> _onRemoveNode;

        public TechnologiesTreeEditorNode(Technology node, Vector2 position, float width, float height,
            GUIStyle nodeStyle, GUIStyle nodeSelectedStyle, GUIStyle connectionStyle,
            Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
            Action<TechnologiesTreeEditorNode> onClickSelectNode, Action<TechnologiesTreeEditorNode> onClickRemoveNode)
        {
            DataNode = node;
            Title = node.Name;
            _rect = new Rect(position.x, position.y, width, height);
            _style = nodeStyle;
            _defaultNodeStyle = nodeStyle;
            _selectedNodeStyle = nodeSelectedStyle;
            _onSelectNode = onClickSelectNode;
            _onRemoveNode = onClickRemoveNode;

            _inPoint = node.Name == "ROOT"
                ? null
                : new ConnectionPoint(this, ConnectionPointType.In, connectionStyle, onClickInPoint);
            _outPoint = new ConnectionPoint(this, ConnectionPointType.Out, connectionStyle, onClickOutPoint);
        }

        public void Draw()
        {
            Title = DataNode.Name;
            
            if (_inPoint != null)
                _inPoint.Draw();
            _outPoint.Draw();
            GUI.Box(_rect, Title, _style);
        }
        public void Drag(Vector2 delta)
        {
            _rect.position += delta;
        }
        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (_rect.Contains(e.mousePosition))
                        {
                            _isDragged = true;
                            _isSelected = true;
                            _style = _selectedNodeStyle;
                            if (_onSelectNode != null)
                                _onSelectNode(this);
                            return true;
                        }

                        _isSelected = false;
                        _style = _defaultNodeStyle;
                        return true;
                    }
                    if (e.button == 1 && _isSelected && _rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    _isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && _isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }

                    break;
            }

            return false;
        }
        private void ProcessContextMenu()
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }
        private void OnClickRemoveNode()
        {
            if (_onRemoveNode != null)
                _onRemoveNode(this);
        }
    }
}