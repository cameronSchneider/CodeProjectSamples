using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GestureRecognizer
{

    /// <summary>
    /// Captures player drawing and call the Recognizer to discover which gesture player id.
    /// Calls 'OnRecognize' event when something is recognized.
    /// </summary>
    public class DrawDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public UILineRenderer line;
        private List<UILineRenderer> lines;
        private Recognizer recognizer;

        [Range(0f, 1f)]
        public float scoreToAccept = 0.8f;

        [Range(1, 10)]
        public int minLines = 1;
        public int MinLines { set { minLines = Mathf.Clamp(value, 1, 10); } }

        [Range(1, 10)]
        public int maxLines = 2;
        public int MaxLines { set { maxLines = Mathf.Clamp(value, 1, 10); } }

        public enum RemoveStrategy { RemoveOld, ClearAll }
        public RemoveStrategy removeStrategy;

        public bool clearNotRecognizedLines;

        public bool fixedArea = false;

        public bool in3D = false;

        GestureData data = new GestureData();

        [System.Serializable]
        public class ResultEvent : UnityEvent<RecognitionResult, DrawDetector> { }
        public ResultEvent OnRecognize;

        RectTransform rectTransform;

        // MY VARS
        private Camera cam;
        private GameObject player;

        void Awake()
        {
            removeStrategy = RemoveStrategy.ClearAll;
            maxLines = 6;
            scoreToAccept = 0.8f;
            clearNotRecognizedLines = true;

            // MY VARS
            cam = Camera.main;
            player = GameObject.FindGameObjectWithTag("Player");
            recognizer = GameObject.FindGameObjectWithTag("Recognizer").GetComponent<Recognizer>();

            in3D = true;
            line.relativeSize = true;
            line.LineList = false;
            lines = new List<UILineRenderer>() { line };
            rectTransform = transform as RectTransform;
            UpdateLines();

            ClearLines();
        }

        void Start()
        {

        }

        void OnValidate()
        {
            maxLines = Mathf.Max(minLines, maxLines);
        }

        public void UpdateLines()
        {
            while (lines.Count < data.lines.Count)
            {
                var newLine = Instantiate(line, line.transform.parent);
                lines.Add(newLine);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].Points = new Vector2[] { };
                lines[i].SetAllDirty();
            }

            int n = Mathf.Min(lines.Count, data.lines.Count);
            for (int i = 0; i < n; i++)
            {
                if (!in3D)
                {
                    lines[i].Points = data.lines[i].points.Select(p => RealToLine(p)).ToArray();
                    lines[i].SetAllDirty();
                }
                else
                {
                    lines[i].Points = data.lines[i].points3D.Select(p => RealToLine(p)).ToArray();
                    lines[i].SetAllDirty();
                }
            }
        }

        Vector2 RealToLine(Vector2 position)
        {
            var local = rectTransform.InverseTransformPoint(position);
            var normalized = Rect.PointToNormalized(rectTransform.rect, local);
            return normalized;
        }

        Vector2 RealToLine(Vector3 position)
        {
            var local = rectTransform.InverseTransformPoint(position);
            var normalized = Rect.PointToNormalized(rectTransform.rect, local);
            return normalized;
        }

        Vector2 FixedPosition(Vector2 position)
        {
            RaycastHit hit = player.GetComponent<SymbolScript>().GetRayHit(position);

            if (hit.collider == null)
                return new Vector2(-999f, -999f);
            else
            {
                return new Vector2(hit.point.x, hit.point.y);
            }
        }

        Vector3 FixedPosition3D(Vector2 position)
        {
            RaycastHit hit = player.GetComponent<SymbolScript>().GetRayHit(position);

            if (hit.collider == null)
                return new Vector3(-999f, -999f, -999f);
            //else if (player.transform.forward == Vector3.forward || player.transform.forward == -Vector3.forward)
            //{
            //    //print("Facing Z Dir    " + hit.point);
            //    return hit.point;
            //}
            //else if (player.transform.forward == Vector3.right || player.transform.forward == -Vector3.right)
            //{
            //    //print("Facing X Dir    " + new Vector3(hit.point.z, hit.point.y, hit.point.x));
            //    //return new Vector3(hit.point.z, hit.point.y, hit.point.x);
            //    return new Vector3();
            //}
            //else
            //{
            //    return new Vector3(-999f, -999f, -999f);
            //}
            else
            {
                return hit.point;
            }
        }

        public void ClearLines()
        {
            data.lines.Clear();
            UpdateLines();
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (data.lines.Count >= maxLines)
            {
                switch (removeStrategy)
                {
                    case RemoveStrategy.RemoveOld:
                        data.lines.RemoveAt(0);
                        break;
                    case RemoveStrategy.ClearAll:
                        data.lines.Clear();
                        break;
                }
            }

            data.lines.Add(new GestureLine());

            if (!in3D)
            {
                var fixedPos = FixedPosition(eventData.position);

                if ((data.LastLine.points.Count == 0 || data.LastLine.points.Last() != fixedPos) && (fixedPos.x != -999f && fixedPos.y != -999f))
                {
                    data.LastLine.points.Add(fixedPos);
                    UpdateLines();
                }
            }
            else
            {
                var fixedPos = FixedPosition3D(eventData.position);

                if ((data.LastLine.points3D.Count == 0 || data.LastLine.points3D.Last() != fixedPos) && (fixedPos.x != -999f && fixedPos.y != -999f))
                {
                    data.LastLine.points3D.Add(fixedPos);
                    if (player.transform.forward == Vector3.right || player.transform.forward == -Vector3.right)
                        data.LastLine.points.Add(new Vector2(fixedPos.z, fixedPos.y));
                    else
                        data.LastLine.points.Add(new Vector2(fixedPos.x, fixedPos.y));

                    UpdateLines();
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!in3D)
            {
                var fixedPos = FixedPosition(eventData.position);

                if ((data.LastLine.points.Count == 0 || data.LastLine.points.Last() != fixedPos) && (fixedPos.x != -999f && fixedPos.y != -999f))
                {
                    data.LastLine.points.Add(fixedPos);
                    UpdateLines();
                }
            }
            else
            {
                var fixedPos = FixedPosition3D(eventData.position);

                if ((data.LastLine.points3D.Count == 0 || data.LastLine.points3D.Last() != fixedPos) && (fixedPos.x != -999f && fixedPos.y != -999f))
                {
                    data.LastLine.points3D.Add(fixedPos);
                    if (player.transform.forward == Vector3.right || player.transform.forward == -Vector3.right)
                        data.LastLine.points.Add(new Vector2(fixedPos.z, fixedPos.y));
                    else
                        data.LastLine.points.Add(new Vector2(fixedPos.x, fixedPos.y));

                    UpdateLines();
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            StartCoroutine(OnEndDragCoroutine(eventData));
        }

        IEnumerator OnEndDragCoroutine(PointerEventData eventData)
        {
            if (!in3D)
            {
                var fixedPos = FixedPosition(eventData.position);

                if (fixedPos.x != -999f && fixedPos.y != -999f)
                    data.LastLine.points.Add(fixedPos);
                UpdateLines();
            }
            else
            {
                var fixedPos = FixedPosition3D(eventData.position);

                if (fixedPos.x != -999f && fixedPos.y != -999f)
                {
                    data.LastLine.points3D.Add(fixedPos);
                    if (player.transform.forward == Vector3.right || player.transform.forward == -Vector3.right)
                        data.LastLine.points.Add(new Vector2(fixedPos.z, fixedPos.y));
                    else
                        data.LastLine.points.Add(new Vector2(fixedPos.x, fixedPos.y));
                }

                UpdateLines();
            }



            for (int size = data.lines.Count; size >= 1 && size >= minLines; size--)
            {
                //last [size] lines
                var sizedData = new GestureData()
                {
                    lines = data.lines.GetRange(data.lines.Count - size, size)
                };

                var sizedNormalizedData = sizedData;


                //Fixed Area is never going to be used, so that line below is commented out as it throws an unnecessary error
                if (fixedArea)
                {
                    var rect = this.rectTransform.rect;
                    sizedNormalizedData = new GestureData()
                    {
                        lines = sizedData.lines.Select(line => new GestureLine()
                        {
                            closedLine = line.closedLine,
                            //points = line.points.Select(p => Rect.PointToNormalized(rect, this.rectTransform.InverseTransformPoint(p))).ToList()
                        }).ToList()
                    };
                }

                RecognitionResult result = null;

                //run in another thread

                var thread = new System.Threading.Thread(() =>
                {
                    result = recognizer.Recognize(sizedNormalizedData, normalizeScale: !fixedArea);
                });

                thread.Start();

                while (thread.IsAlive)
                {
                    yield return null;
                }

                if (result.gesture != null && result.score.score >= scoreToAccept)
                {
                    OnRecognize.Invoke(result, this);
                    if (clearNotRecognizedLines)
                    {
                        data = sizedData;
                        UpdateLines();
                    }
                    break;
                }
                else
                {
                    if (data.lines.Count > 1)
                        OnRecognize.Invoke(RecognitionResult.Empty, this);
                }

            }

            yield return null;
        }
    }
}