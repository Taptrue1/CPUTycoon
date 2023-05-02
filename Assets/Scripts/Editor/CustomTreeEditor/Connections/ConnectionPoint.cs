using System;
using UnityEngine;

namespace Editor.CustomTreeEditor.Connections
{
    public class ConnectionPoint
    {
        public Rect Rect { get => _rect; }
        public GUIStyle Style { get => _style; }
        public ConnectionPointType Type { get => _type; }
        public TechnologiesTreeEditorNode Node { get => _node; }
        public Action<ConnectionPoint> OnClickConnectionPoint { get => _onClickConnectionPoint; }
        
        private Rect _rect;
        private GUIStyle _style;
        private ConnectionPointType _type;
        private TechnologiesTreeEditorNode _node;
        private Action<ConnectionPoint> _onClickConnectionPoint;

        public ConnectionPoint(TechnologiesTreeEditorNode node, ConnectionPointType type, GUIStyle style,
            Action<ConnectionPoint> onClickConnectionPoint)
        {
            _node = node;
            _type = type;
            _style = style;
            _onClickConnectionPoint = onClickConnectionPoint;
            _rect = new Rect(0, 0, 20f, 12f);
        }

        public void Draw()
        {
            _rect.y = _node.Rect.y + _node.Rect.height / 2f - _rect.height / 2f;

            switch (_type)
            {
                case ConnectionPointType.In:
                    _rect.x = _node.Rect.x - _rect.width / 2f;
                    break;
                case ConnectionPointType.Out:
                    _rect.x = _node.Rect.x + _node.Rect.width - _rect.width / 2f;
                    break;
            }

            if (GUI.Button(_rect, "", _style))
            {
                if (_onClickConnectionPoint != null)
                    _onClickConnectionPoint(this);
            }
        }
    }

    public enum ConnectionPointType
    {
        In,
        Out
    }
}