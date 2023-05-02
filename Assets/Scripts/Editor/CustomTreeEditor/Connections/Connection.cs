using System;
using Core.Datas;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomTreeEditor.Connections
{
    public class Connection
    {
        public ConnectionPoint InPoint { get => _inPoint; }
        public ConnectionPoint OutPoint { get => _outPoint; }
        public Action<Connection> OnClickRemoveConnection { get => _onClickRemoveConnection; }
        public TechnologyNodeData Source { get => _source; }
        public TechnologyNodeData Target { get => _target; }
        
        private ConnectionPoint _inPoint;
        private ConnectionPoint _outPoint;
        private Action<Connection> _onClickRemoveConnection;
        private TechnologyNodeData _source;
        private TechnologyNodeData _target;

        public Connection(TechnologyNodeData source, TechnologyNodeData target, ConnectionPoint inPoint,
            ConnectionPoint outPoint, Action<Connection> onClickRemoveConnection)
        {
            _source = source;
            _target = target;
            _inPoint = inPoint;
            _outPoint = outPoint;
            _onClickRemoveConnection = onClickRemoveConnection;
        }

        public void Draw()
        {
            Handles.DrawBezier(_inPoint.Rect.center, _outPoint.Rect.center, _inPoint.Rect.center + Vector2.left * 50f,
                _outPoint.Rect.center - Vector2.left * 50f, Color.white, null, 2f);

            if (!Handles.Button((_inPoint.Rect.center + _outPoint.Rect.center) * 0.5f, Quaternion.identity, 4, 8,
                    Handles.RectangleHandleCap)) return;
            
            if (_onClickRemoveConnection != null) _onClickRemoveConnection(this);
        }
    }
}