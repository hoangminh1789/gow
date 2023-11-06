using UnityEngine;
using UnityEditor;

namespace Artemis.Utilities
{
	[InitializeOnLoad]
	[ExecuteInEditMode]
	public class ReorderComponents : EditorWindow
	{
		int		_activeButton	= 0;
		int		_tempIndex		= 0;
		int		_lastIndex		= 0;
		bool	_mouseDown		= false;
		int[]	_newIndexes		= null;

		static Texture2D	_activeBackground		= null;
		static GUIStyle		_styleButton			= null;
		static GUIStyle		_styleButtonDisabled	= null;
		static GUIStyle		_styleButtonActive		= null;

		[MenuItem("Tools/Reorder Components")]
		static void Show()
		{
			EditorWindow window = EditorWindow.GetWindow(typeof(ReorderComponents));
			ReorderComponents windowHndle = (ReorderComponents)window;
			window.titleContent = new GUIContent("Reorder Components");
			windowHndle.autoRepaintOnSceneChange = true;
		}

		void OnInspectorUpdate()
		{
			Repaint();
		}

		void OnGUI()
		{
			Transform currentTransform = Selection.activeTransform;

			if (currentTransform != null)
			{
				if (_activeBackground = null)
				{
					_activeBackground	= new Texture2D(1, 1);
					_activeBackground.SetPixel(0, 0, Color.gray);
					_activeBackground.Apply();
				}

				Component[] comps = currentTransform.GetComponents<Component>();

				if (_styleButton == null)
				{
					_styleButton				= new GUIStyle(GUI.skin.button);
					_styleButton.alignment		= TextAnchor.MiddleLeft;
					_styleButton.fixedHeight	= 25;
					_styleButton.margin			= new RectOffset(4, 4, 4, 4);
				}

				if(_styleButtonDisabled == null)
                {
					_styleButtonDisabled					= new GUIStyle(GUI.skin.button);
					_styleButtonDisabled.alignment			= TextAnchor.MiddleLeft;
					_styleButtonDisabled.fixedHeight		= 25;
					_styleButtonDisabled.margin				= new RectOffset(4, 4, 4, 4);
					_styleButtonDisabled.normal.textColor	= Color.gray;
                }

				if(_styleButtonActive == null)
                {
					_styleButtonActive						= new GUIStyle();
					_styleButtonActive.alignment			= TextAnchor.MiddleLeft;
					_styleButtonActive.fixedHeight			= 25;
					_styleButtonActive.margin				= new RectOffset(4, 4, 4, 4);
					_styleButtonActive.normal.background	= _activeBackground;
					_styleButtonActive.normal.textColor		= Color.gray;
                }

				if (_mouseDown == false && Event.current.type == EventType.MouseDown)
				{
					_activeButton = Mathf.FloorToInt(Event.current.mousePosition.y / 29);

					if (comps[_activeButton].GetType().ToString() == "UnityEngine.Transform")
					{
						_mouseDown = false;
					}
					else
					{
						_mouseDown	= true;
						_lastIndex	= _activeButton;
						_newIndexes = new int[comps.Length];

						for (int i = 0; i < comps.Length; i++)
						{
							_newIndexes[i] = i;
						}
					}
				}

				// Mouse button released
				if (_mouseDown && Event.current.type == EventType.MouseUp)
				{
					_mouseDown = false;

					// Reorder components
					int positionsToMove = Mathf.RoundToInt(Mathf.Abs(_tempIndex - _activeButton));

					if (positionsToMove > 0)
					{
						int direction = (_tempIndex - _activeButton) / Mathf.Abs(_tempIndex - _activeButton);

						for (int i = 0; i < positionsToMove; i++)
						{
							if (direction > 0)
							{
								UnityEditorInternal.ComponentUtility.MoveComponentDown(comps[_activeButton + i]);
							}
							if (direction < 0)
							{
								UnityEditorInternal.ComponentUtility.MoveComponentUp(comps[_activeButton - i]);
							}

							comps = currentTransform.GetComponents<Component>();
						}
					}
				}

				// Draw buttons
				for (int i = 0; i < comps.Length; i++)
				{
					int j = i;

					if (_mouseDown)
					{
						j = _newIndexes[i];
					}

					string sName = comps[j].GetType().ToString();

					GUIStyle style = _styleButton;

					if (_mouseDown && i == _tempIndex)
					{
						style = _styleButtonActive; 
					}
					if (sName == "UnityEngine.Transform")
					{
						style = _styleButtonDisabled; 
					}

					GUILayout.Button(sName, style);
				}

				// If mouse button is down, draw the extra button
				if (_mouseDown)
				{
					Rect pos = new Rect(Event.current.mousePosition.x - Screen.width / 2, Event.current.mousePosition.y - 12.5f, Screen.width - 10, 25);
					GUI.Button(pos, comps[_activeButton].GetType().ToString(), _styleButton);
				}

				// Get index for the new temp button
				if (_mouseDown)
				{
					int tmp = Mathf.FloorToInt(Event.current.mousePosition.y / 29);

					if (tmp > comps.Length - 1)
					{
						tmp = comps.Length - 1;
					}

					if (tmp < 0)
					{
						tmp = 0;
					}

					if (comps[tmp].GetType().ToString() != "UnityEngine.Transform")
					{
						_tempIndex = tmp;
					}

					if (_tempIndex != _lastIndex)
					{
						int temp = _newIndexes[_lastIndex];

						_newIndexes[_lastIndex]		= _newIndexes[_tempIndex];
						_newIndexes[_tempIndex]		= temp;

						_lastIndex	= _tempIndex;
					}
				}
			}
		}
	}
}