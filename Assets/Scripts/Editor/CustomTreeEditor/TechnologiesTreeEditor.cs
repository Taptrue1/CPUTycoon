using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Datas;
using Editor.CustomTreeEditor.Connections;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomTreeEditor
{
    public class TechnologiesTreeEditor : EditorWindow
    {
        private bool _isResizing;
        private bool _selectingNode;
        private float _sidePanelWidthRatio = 0.3f;
        private Rect _resizer;
        private Vector2 _drag;
        private Vector2 _offset;
        private GUIStyle _sidePanelStyle;
        private GUIStyle _resizerStyle;

        private int _nodeWidth = 100;
        private int _nodeHeight = 50;
        private int _newNodeLastIndex;
        private string _treeSavePath;
        private GUIStyle _nodeStyle;
        private GUIStyle _nodeSelectedStyle;
        private TechnologiesTreeEditorNode _selectedNode;

        private GUIStyle _connectionStyle;
        private ConnectionPoint _selectedInPoint;
        private ConnectionPoint _selectedOutPoint;
        private List<Connection> _connections;

        private TechnologyNodeData _rootNode;
        private List<TechnologiesTreeEditorNode> _editorNodes;
        
        private const int SidePanelHeightOffset = 20;

        [MenuItem("Custom Windows/Technologies Tree Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<TechnologiesTreeEditor>();
            window.titleContent = new GUIContent("Technologies Tree Editor");
        }
        
        private void OnGUI()
        {
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawNodes();
            DrawConnections();
            DrawConnectionLine(Event.current);
            
            DrawSidePanel(SidePanelHeightOffset);
            DrawResizer(SidePanelHeightOffset);
            DrawTopPanel();
            
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if(GUI.changed) Repaint();
        }
        private void OnEnable()
        {
            _nodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D,
                    textColor = Color.white
                },
                border = new RectOffset(12, 12, 12, 12),
                alignment = TextAnchor.MiddleCenter
            };

            _nodeSelectedStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D,
                    textColor = Color.white
                },
                border = new RectOffset(12, 12, 12, 12),
                alignment = TextAnchor.MiddleCenter
            };

            _sidePanelStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/darkviewbackground.png") as Texture2D,
                    textColor = Color.white
                },
                border = new RectOffset(12, 12, 12, 12)
            };

            _resizerStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/toolbar button on.png") as Texture2D
                }
            };
            
            _connectionStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn.png") as Texture2D
                },
                active =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn on.png") as Texture2D
                },
                border = new RectOffset(4, 4, 12, 12)
            };
        }
        private void OnDisable()
        {
            SaveTree();
        }

        #region MainMethods
        
        private void CreateGraph()
        {
            var y = 0;
            var x = 0;
            var offsetY = 40f;
            var offsetX = 40f;
            var paddingY = 50f;
            var paddingX = (_sidePanelWidthRatio * position.width) + 50f;
            var nodeXSpacing = _nodeWidth + offsetX * 2;
            var nodeYSpacing = _nodeHeight + offsetY * 2;
            var currentNodes = new List<TechnologyNodeData> {_rootNode};
            var nodesMapping = new Dictionary<string, TechnologiesTreeEditorNode>();
            var edges = new List<(TechnologyNodeData, TechnologyNodeData)>();

            _editorNodes = new List<TechnologiesTreeEditorNode>();
            _newNodeLastIndex = 0;

            while (currentNodes.Count > 0)
            {
                var newNodes = new List<TechnologyNodeData>();
                foreach (var node in currentNodes)
                {
                    var editorNode = new TechnologiesTreeEditorNode(
                        node,
                        new Vector2(paddingX + x * nodeXSpacing, paddingY + y * nodeYSpacing),
                        _nodeWidth,
                        _nodeHeight,
                        _nodeStyle,
                        _nodeSelectedStyle,
                        _connectionStyle,
                        OnClickInPoint,
                        OnClickOutPoint,
                        OnClickSelectNode,
                        OnClickRemoveNode
                    );
                    nodesMapping[node.Name] = editorNode;
                    _editorNodes.Add(editorNode);

                    edges.AddRange(node.Children.Select(child => (node, child)));
                    newNodes.AddRange(node.Children.Where(child =>
                        child != null && !newNodes.Any(node => node.Name == child.Name)));
                    y++;
                }
                currentNodes = newNodes;
                x++;
                y = 0;
            }
            
            foreach (var (source, target) in edges)
            {
                _connections.Add(new Connection(
                    source,
                    target,
                    nodesMapping[target.Name].InPoint,
                    nodesMapping[source.Name].OutPoint,
                    OnClickRemoveConnection
                ));
            }
        }
        private void DrawGrid(int gridSpacing, float gridOpacity, Color color)
        {
            var widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            var heightDivs = Mathf.CeilToInt((position.height) / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(color.r, color.g, color.b, gridOpacity);

            _offset += _drag * 0.5f;
            var newOffset = new Vector3(_offset.x % gridSpacing, (_offset.y) % gridSpacing, 0);

            for (var i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(
                    new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,
                    new Vector3(gridSpacing * i, position.height, 0f) + newOffset
                );
            }

            for (var j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(
                    new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,
                    new Vector3(position.width, gridSpacing * j, 0f) + newOffset
                );
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }
        private void DrawSidePanel(float heightOffset)
        {
            var sidePanelWidth = _sidePanelWidthRatio * position.width;

            GUI.Box(new Rect(0, heightOffset, sidePanelWidth, position.height - heightOffset), "", _sidePanelStyle);
            GUILayout.BeginArea(new Rect(5, heightOffset + 5, sidePanelWidth - 10, position.height - 10 - heightOffset));
            EditorGUILayout.LabelField("Technology Node Settings", EditorStyles.boldLabel);
            
            if (_selectedNode == null)
            {
                EditorGUILayout.HelpBox("Select a node", MessageType.Info);
                GUILayout.EndArea();
                return;
            }
            
            var node = _selectedNode.DataNode;
            if (node.Name == "ROOT")
            {
                EditorGUILayout.HelpBox("Can't edit root node!", MessageType.Warning);
            }
            else
            {
                var fields = node.GetType().GetFields();
                foreach (var field in fields)
                {
                    if (field.IsStatic) continue;
                    if (field.Name == "Children") continue;
                    
                    
                    switch (field.GetValue(node))
                    {
                        case int intValue:
                            var newIntValue = EditorGUILayout.IntField(field.Name, intValue);
                            if(newIntValue != intValue)
                                field.SetValue(node, newIntValue);
                            break;
                        case string stringValue:
                            var newStringValue = EditorGUILayout.TextField(field.Name, stringValue);
                            if(newStringValue != stringValue)
                                field.SetValue(node, newStringValue);
                            break;
                        default:
                            EditorGUILayout.LabelField(field.Name, "Unsupported field type");
                            break;
                    }
                }
            }
            
            GUILayout.EndArea();
        }
        private void DrawResizer(float heightOffset)
        {
            _resizer = new Rect((position.width * _sidePanelWidthRatio) - 4f, heightOffset, 10f, position.height - heightOffset);

            GUILayout.BeginArea(new Rect(_resizer.position, new Vector2(4, position.height - heightOffset)), _resizerStyle);
            GUILayout.EndArea();
            EditorGUIUtility.AddCursorRect(_resizer, MouseCursor.ResizeHorizontal);
        }
        private void DrawTopPanel()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("Выбрать действие", EditorStyles.toolbarDropDown, GUILayout.Width(position.width)))
            {
                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("Открыть"), false, OpenExistingFile);
                menu.AddItem(new GUIContent("Создать"), false, CreateNewFile);
                menu.ShowAsContext();
            }

            GUILayout.EndHorizontal();
        }
        private void DrawNodes()
        {
            if (_editorNodes == null) return;
            
            foreach(var node in _editorNodes)
                node.Draw();
        }
        private void DrawConnections()
        {
            if (_connections == null) return;
            
            foreach (var connection in _connections)
                connection.Draw();
        }
        private void DrawConnectionLine(Event e)
        {
            var currentPoint = _selectedInPoint ?? _selectedOutPoint;
            
            if (currentPoint == null) return;

            Handles.DrawBezier(currentPoint.Rect.center, e.mousePosition, currentPoint.Rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f, Color.white, null, 2f);
            GUI.changed = true;
        }

        #endregion
        
        #region FileProcessing
        
        private void OpenExistingFile()
        {
            if(_rootNode != null) SaveTree();
            
            var path = EditorUtility.OpenFilePanel("Выберите файл", Application.dataPath, "json");

            if (string.IsNullOrEmpty(path)) return;
            if (!File.Exists(path)) return;


            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Open);
            var rootNode = (TechnologyNodeData)formatter.Deserialize(stream);

            stream.Close();

            _editorNodes = null;
            _rootNode = rootNode;
            _treeSavePath = path;
            _connections = new List<Connection>();

            CreateGraph();
        }
        private void CreateNewFile()
        {
            if(_rootNode != null) SaveTree();
            
            var path = EditorUtility.SaveFilePanel("Создать файл", Application.dataPath, "NewTree", "json");
            var rootNode = new TechnologyNodeData("ROOT") { Children = new()
            {
                new("TestNode1"), 
                new("TestNode2")
            }};

            _editorNodes = null;
            _rootNode = rootNode;
            _treeSavePath = path;
            _connections = new List<Connection>();

            CreateGraph();
        }
        private void SaveTree()
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(_treeSavePath, FileMode.Create);
            
            formatter.Serialize(stream, _rootNode);
            stream.Close();
            
            Debug.Log("Tree Saved");
        }
        
        #endregion
        
        #region EventsProcessing

        private void ProcessEvents(Event e)
        {
            _drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        ClearConnectionSelection();
                        if (_selectedNode != null && !_selectingNode)
                        {
                            if (e.mousePosition.x > _sidePanelWidthRatio * position.width)
                                _selectedNode = null;
                        }

                        if (e.button == 0 && _resizer.Contains(e.mousePosition))
                        {
                            _isResizing = true;
                        }
                    }
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;

                case EventType.MouseUp:
                    _selectingNode = false;
                    _isResizing = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && !_isResizing)
                        OnDrag(e.delta);
                    break;
            }

            if (_isResizing) Resize(e);
        }
        private void ProcessNodeEvents(Event e)
        {
            if (_editorNodes == null) return;
            for (var i = _editorNodes.Count - 1; i >= 0; i--)
            {
                var guiChanged = _editorNodes[i].ProcessEvents(e);
                if (guiChanged) GUI.changed = true;
            }
        }
        private void ProcessContextMenu(Vector2 mousePosition)
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
            genericMenu.ShowAsContext();
        }
        private void Resize(Event e)
        {
            _sidePanelWidthRatio = e.mousePosition.x / position.width;
            GUI.changed = true;
        }
        private void CreateConnection()
        {
            var source = _selectedOutPoint.Node.DataNode;
            var target = _selectedInPoint.Node.DataNode;
            
            source.Children.Add(target);
            _connections.Add(new Connection(
                source,
                target,
                _selectedInPoint,
                _selectedOutPoint,
                OnClickRemoveConnection
            ));
        }
        private void ClearConnectionSelection()
        {
            _selectedInPoint = null;
            _selectedOutPoint = null;
        }
        private void OnDrag(Vector2 delta)
        {
            _drag = delta;
            foreach(var node in _editorNodes)
                node.Drag(delta);
            GUI.changed = true;
        }
        private void OnClickInPoint(ConnectionPoint inPoint)
        {
            _selectedInPoint = inPoint;

            if (_selectedOutPoint == null) return;
            if (_selectedOutPoint.Node != _selectedInPoint.Node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
        private void OnClickOutPoint(ConnectionPoint outPoint)
        {
            _selectedOutPoint = outPoint;

            if (_selectedInPoint == null) return;
            if (_selectedOutPoint.Node != _selectedInPoint.Node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
        private void OnClickRemoveConnection(Connection connection)
        {
            connection.Source.Children.Remove(connection.Target);
            _connections.Remove(connection);
        }
        private void OnClickAddNode(Vector2 mousePosition)
        {
            var name = $"Untitled_{_newNodeLastIndex:00}";
            var node = new TechnologyNodeData(name);
            
            _newNodeLastIndex++;
            _editorNodes.Add(new TechnologiesTreeEditorNode(
                node,
                mousePosition,
                _nodeWidth,
                _nodeHeight,
                _nodeStyle,
                _nodeSelectedStyle,
                _connectionStyle,
                OnClickInPoint,
                OnClickOutPoint,
                OnClickSelectNode,
                OnClickRemoveNode
            ));
        }
        private void OnClickRemoveNode(TechnologiesTreeEditorNode node)
        {
            var connectionsToRemove = _connections.Where(c => c.InPoint == node.InPoint || c.OutPoint == node.OutPoint).ToList();

            foreach (var child in connectionsToRemove)
            {
                child.Source.Children.Remove(child.Target);
                _connections.Remove(child);
            }

            if (_selectedNode == node)
                _selectedNode = null;
            
            _editorNodes.Remove(node);
        }
        private void OnClickSelectNode(TechnologiesTreeEditorNode node)
        {
            _selectedNode = node;
            _selectingNode = true;
            GUI.changed = true;
        }

        #endregion
    }
}