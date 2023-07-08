using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class UiLineRenderer : Graphic
    {
        [SerializeField] private float _thickness = 1f;

        private float _width;
        private float _height;
        private float _unitWidth;
        private float _unitHeight;
        private Vector2Int _gridSize;
        private List<Line> _lines;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (canvasRenderer == null)
            {
                Debug.LogWarning("UILineRenderer OnPopulateMesh: canvasRenderer is null");
                return;
            }
            
            vh.Clear();
            
            _width = rectTransform.rect.width;
            _height = rectTransform.rect.height;
            _unitWidth = _width / _gridSize.x;
            _unitHeight = _height / _gridSize.y;
            
            if(_lines == null || _lines.Count == 0) return;
            foreach (var line in _lines)
                DrawLine(line.StartPoint, line.EndPoint, vh);
        }

        public void SetGridSize(Vector2Int gridSize)
        {
            _gridSize = gridSize;
        }
        public void SetLines(List<Line> lines)
        {
            _lines = lines;
            SetAllDirty();
        }
        
        private void DrawLine(Vector3 startPos, Vector3 endPos, VertexHelper vh)
        {
            var angle = GetAngle(endPos, startPos);
            var rotation = Quaternion.Euler(0, 0, angle);
            var thicknessOffset = rotation * new Vector3(0, -_thickness / 2, 0);
            var startIndex = vh.currentVertCount;
            var vert = UIVertex.simpleVert;
            vert.color = color;

            vert.position = startPos + thicknessOffset;
            vh.AddVert(vert);
            vert.position = startPos - thicknessOffset;
            vh.AddVert(vert);

            vert.position = endPos - thicknessOffset;
            vh.AddVert(vert);
            vert.position = endPos + thicknessOffset;
            vh.AddVert(vert);

            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
        private void DrawPoint(Vector2 point, VertexHelper vh)
        {
            var vert = UIVertex.simpleVert;
            var position = new Vector3(point.x * _unitWidth, point.y * _unitHeight);
            vert.color = color;
            
            vert.position = new Vector3(-_thickness / 2, 0) + position;
            vh.AddVert(vert);

            vert.position = new Vector3(_thickness / 2, 0) + position;
            vh.AddVert(vert);
        }
        private float GetAngle(Vector2 me, Vector2 target)
        {
            return Mathf.Atan2(target.y - me.y, target.x - me.x) * Mathf.Rad2Deg;
        }
    }

    public class Line
    {
        public Vector2 StartPoint { get; }
        public Vector2 EndPoint { get; }
        
        public Line(Vector2 startPoint, Vector2 endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
    }
}