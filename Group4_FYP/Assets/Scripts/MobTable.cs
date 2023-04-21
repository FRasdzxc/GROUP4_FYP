using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HugeScript;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace HugeScript
{
[CreateAssetMenu(fileName = "Mob Table", menuName = "Huge Script/Mob Table SO")]
    public class MobTable : ScriptableObject
    {
        public DropChangeItem[] GuaranteedLootTable;
        public DropChangeItem[] OneItemFromList = new DropChangeItem[1];
        public float WeightToNoDrop = 100;

        // Return List of Guaranteed Drop 
        public List<GameObject> GetGuaranteeedLoot()
        {
            List<GameObject> lootList = new List<GameObject>();

            for (int i = 0; i < GuaranteedLootTable.Length; i++)
            {
                // Adds the drawn number of items to drop
                int count = Random.Range(GuaranteedLootTable[i].MinCountItem, GuaranteedLootTable[i].MaxCountItem);
                for (int j = 0; j < count; j++)
                {
                    lootList.Add(GuaranteedLootTable[i].Drop);
                }
            }

            return lootList;
        }

        // Return List of Optional Drop 
        public List<GameObject> GetRandomLoot(int ChangeCount)
        {
            List<GameObject> lootList = new List<GameObject>();
            float totalWeight = WeightToNoDrop;

            // Executes a function a specified number of times
            for (int j = 0; j < ChangeCount; j++)
            {
                // They add up the entire weight of the items
                for (int i = 0; i < OneItemFromList.Length; i++)
                {
                    totalWeight += OneItemFromList[i].Weight;
                }

                float value = Random.Range(0, totalWeight);
                float timed_value = 0;

                for (int i = 0; i < OneItemFromList.Length; i++)
                {
                    // If timed_value is greater than value, it means this item has been drawn
                    timed_value += OneItemFromList[i].Weight;
                    if (timed_value > value)
                    {
                        int count = Random.Range(OneItemFromList[i].MinCountItem, OneItemFromList[i].MaxCountItem + 1);
                        for (int c = 0; c < count; c++)
                        {
                            lootList.Add(OneItemFromList[i].Drop);
                        }
                        break;
                    }
                }
            }

            return lootList;
        }


        public void SpawnDrop(Tilemap _tileMap, int _count, float _rangeX, float _rangeY)
        {
            List<GameObject> guaranteed = GetGuaranteeedLoot();
            List<GameObject> randomLoot = GetRandomLoot(_count);
            List<Vector3> availableTiles = new List<Vector3>();

            for(int n = _tileMap.cellBounds.xMin; n < _tileMap.cellBounds.xMax; n++)
            {
                for(int p = _tileMap.cellBounds.yMin; p < _tileMap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = new Vector3Int(n, p, (int)_tileMap.transform.position.y);
                    Vector3 place = _tileMap.CellToWorld(localPlace);
                    if (_tileMap.HasTile(localPlace))
                    {
                        availableTiles.Add(place);
                    }
                }
            }

            for (int i = 0; i < guaranteed.Count; i++)
            {
                int temp = Random.Range(0, availableTiles.Count);
                Vector3 _position = new Vector3(availableTiles[temp].x + 0.5f, availableTiles[temp].y + 0.5f, availableTiles[temp].z);
                Instantiate(guaranteed[i], new Vector3(_position.x + Random.Range(-_rangeX, _rangeX), _position.y + Random.Range(-_rangeY, _rangeY), -1), Quaternion.identity);
            }

            for (int i = 0; i < randomLoot.Count; i++)
            {
                int temp = Random.Range(0, availableTiles.Count);
                Vector3 _position = new Vector3(availableTiles[temp].x + 0.5f, availableTiles[temp].y + 0.5f, availableTiles[temp].z);
                Instantiate(randomLoot[i], new Vector3(_position.x + Random.Range(-_rangeX, _rangeX), _position.y + Random.Range(-_rangeY, _rangeY), -1), Quaternion.identity);
            }
        }
    }

    /* --------------------- */
    // Drop Item Change Class
    /* --------------------- */

    [System.Serializable]
    public class DropChangeItem
    {
        public float Weight;
        public GameObject Drop;
        public int MinCountItem;
        public int MaxCountItem;
    }
}
    


/* --------------------- */
// Custom Editor & Property Draw (look)
/* --------------------- */


#if UNITY_EDITOR

/* --------------------- */
// Custom Editor
/* --------------------- */

[CustomEditor(typeof(MobTable))]
public class MobEditor : Editor
{
    // Guaranteed
    SerializedProperty guaranteedList;
    ReorderableList reorderableGuaranteed;
    // Change
    SerializedProperty changeToGetList;
    ReorderableList reorderableChange;

    MobTable ld;

    private void OnEnable()
    {
        /* GUARANTEED */
        guaranteedList = serializedObject.FindProperty("GuaranteedLootTable");
        reorderableGuaranteed = new ReorderableList(serializedObject, guaranteedList, true, true, true, true);
        // Functions
        reorderableGuaranteed.drawElementCallback = DrawGuaranteedListItems;
        reorderableGuaranteed.drawHeaderCallback = DrawHeaderGuaranteed;

        /* Change */
        changeToGetList = serializedObject.FindProperty("OneItemFromList");
        reorderableChange = new ReorderableList(serializedObject, changeToGetList, true, true, true, true);
        // Functions
        reorderableChange.drawElementCallback += DrawChangeListItems;
        reorderableChange.drawHeaderCallback += DrawHeaderChange;

        ld = target as MobTable;
    }



    void DrawHeaderGuaranteed(Rect rect) { EditorGUI.LabelField(rect, "Guaranteed Mob Table"); }
    void DrawHeaderChange(Rect rect) { EditorGUI.LabelField(rect, "Chance To Spawn Boss Table"); }

    void DrawGuaranteedListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        MobTable loot = (MobTable)target;
        reorderableGuaranteed.elementHeight = 42;

        SerializedProperty element = reorderableGuaranteed.serializedProperty.GetArrayElementAtIndex(index);


        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), element, GUIContent.none);

        //LootDrop loot = (LootDrop)target;
    }

    void DrawChangeListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        reorderableChange.elementHeight = 42;

        SerializedProperty element = reorderableChange.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), element, GUIContent.none);
    }

    public override void OnInspectorGUI()
    {

        EditorUtility.SetDirty(target);
        MobTable loot = (MobTable)target;
        EditorGUILayout.BeginVertical("box");

        EditorGUI.indentLevel = 0;

        // Loot Name
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.textColor = GUI.color;
        myStyle.alignment = TextAnchor.UpperCenter;
        myStyle.fontStyle = FontStyle.Bold;
        int _ti = myStyle.fontSize;

        EditorGUILayout.LabelField($"Mob Table", myStyle);


        myStyle.fontStyle = FontStyle.Italic;
        myStyle.fontSize = 20;

        EditorGUILayout.LabelField($",,{loot.name}''", myStyle);

        EditorGUILayout.Space(10);

        EditorGUI.BeginChangeCheck();

        /* Fixed by D9Construct */
            serializedObject.Update();
            ValidateGuaranteedList(loot);
            reorderableGuaranteed.DoLayoutList();
            ValidateOneItemFromList(loot);
            reorderableChange.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                for (int index = 0; index < loot.OneItemFromList.Length; index++)
                {
                    SerializedProperty OIFElement = reorderableChange.serializedProperty.GetArrayElementAtIndex(index);
                    loot.OneItemFromList[index].Weight = OIFElement.FindPropertyRelative("Weight").floatValue;
                    loot.OneItemFromList[index].Drop = (GameObject)OIFElement.FindPropertyRelative("Drop").objectReferenceValue;
                    loot.OneItemFromList[index].MinCountItem = OIFElement.FindPropertyRelative("MinCountItem").intValue;
                    loot.OneItemFromList[index].MaxCountItem = OIFElement.FindPropertyRelative("MaxCountItem").intValue;
                }
                for (int index = 0; index < loot.GuaranteedLootTable.Length; index++)
                {
                    SerializedProperty GuaranteedElement = reorderableGuaranteed.serializedProperty.GetArrayElementAtIndex(index);
                    loot.GuaranteedLootTable[index].Weight = GuaranteedElement.FindPropertyRelative("Weight").floatValue;
                    loot.GuaranteedLootTable[index].Drop = (GameObject)GuaranteedElement.FindPropertyRelative("Drop").objectReferenceValue;
                    loot.GuaranteedLootTable[index].MinCountItem = GuaranteedElement.FindPropertyRelative("MinCountItem").intValue;
                    loot.GuaranteedLootTable[index].MaxCountItem = GuaranteedElement.FindPropertyRelative("MaxCountItem").intValue;
                }
            }
        /* Fixed by D9Construct */

        // Nothing Weight
        loot.WeightToNoDrop = EditorGUILayout.FloatField("No Drop Weight", loot.WeightToNoDrop);

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            Rect r = EditorGUILayout.BeginVertical("box");
            myStyle.fontStyle = FontStyle.Bold;
            myStyle.fontSize = 20;

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField($"Drop Change", myStyle);

            float totalWeight = loot.WeightToNoDrop;
            float guaranteedHeight = 0;

            if (loot.OneItemFromList != null)
            {
                for (int j = 0; j < loot.OneItemFromList.Length; j++)
                {
                    totalWeight += loot.OneItemFromList[j].Weight;
                }
            }

            
            var _oldColor = GUI.backgroundColor;

            if (0 < loot.GuaranteedLootTable.Length) { guaranteedHeight += 10; }

            /* Guaranteed */
            GUI.backgroundColor = Color.green;
            for (int i = 0; i < loot.GuaranteedLootTable.Length; i++)
            {
                string _tmpString = "";
                guaranteedHeight += 25;
                if (loot.GuaranteedLootTable[i].Drop == null) { _tmpString = " --- No Drop Object --- "; } else { _tmpString = loot.GuaranteedLootTable[i].Drop.name; }
                EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i), r.width - 10, 20), 1, $"{_tmpString} [{loot.GuaranteedLootTable[i].MinCountItem}-{loot.GuaranteedLootTable[i].MaxCountItem}]   -   Guaranteed");
            }
            GUI.backgroundColor = _oldColor;

            /* Not Guaranteed */
            for (int i = 0; i < loot.OneItemFromList.Length; i++)
            {
                string _tmpString = "";
                if (loot.OneItemFromList[i].Drop == null) { _tmpString = "!!! No Drop Object Attackhment !!!"; } else { _tmpString = loot.OneItemFromList[i].Drop.name; }
                if (loot.OneItemFromList[i].Weight / totalWeight < 0) { 
                    GUI.backgroundColor = Color.red;
                EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i) + guaranteedHeight, r.width - 10, 20), 1, "Error");
                } else {
                    EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i) + guaranteedHeight, r.width - 10, 20), loot.OneItemFromList[i].Weight / totalWeight, $"{_tmpString} [{loot.OneItemFromList[i].MinCountItem}-{loot.OneItemFromList[i].MaxCountItem}]   -   {(loot.OneItemFromList[i].Weight / totalWeight * 100).ToString("f2")}%");
                }
                GUI.backgroundColor = _oldColor;
            }

            GUI.backgroundColor = Color.gray;
            EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * loot.OneItemFromList.Length + 10) + guaranteedHeight, r.width - 10, 20), loot.WeightToNoDrop / totalWeight, $"Nothing Additional   -   {(loot.WeightToNoDrop / totalWeight * 100).ToString("f2")}%");
            GUI.backgroundColor = _oldColor;

            EditorGUILayout.Space(25 * loot.OneItemFromList.Length + 45 + guaranteedHeight);

            EditorGUILayout.EndVertical();
        }

        void ValidateOneItemFromList(MobTable loot)
        {
            bool _countError = false;
            bool _prefabError = false;
            bool _weightError = false;

            for (int index = 0; index < loot.OneItemFromList.Length; index++)
            {
                if (loot.OneItemFromList[index].Drop == null) { _prefabError = true; }
                if (loot.OneItemFromList[index].MinCountItem <= 0) { _countError = true; }
                if (loot.OneItemFromList[index].MinCountItem > loot.OneItemFromList[index].MaxCountItem) { _countError = true; }
                if (loot.OneItemFromList[index].Weight < 0) { _weightError = true; }
            }
            if (_prefabError == true) { EditorGUILayout.HelpBox("One of the List Items does not have ''Item To Drop'' assigned, which will cause an error if it is drawn", MessageType.Error, true); }
            if (_countError == true) { EditorGUILayout.HelpBox("One of the List Items has an incorrect number of items, which will result in items not appearing when drawn", MessageType.Warning, true); }
            if (_weightError == true) { EditorGUILayout.HelpBox("One of the List Items has an incorrect Weight, this will cause erroneous data readings or the whole system will crash", MessageType.Error, true); }
        }
        void ValidateGuaranteedList(MobTable loot)
        {
            bool _countError = false;
            bool _prefabError = false;
            bool _weightError = false;

            for (int index = 0; index < loot.GuaranteedLootTable.Length; index++)
            {
                if (loot.GuaranteedLootTable[index].Drop == null) { _prefabError = true; }
                if (loot.GuaranteedLootTable[index].MinCountItem <= 0) { _countError = true; }
                if (loot.GuaranteedLootTable[index].MinCountItem > loot.GuaranteedLootTable[index].MaxCountItem) { _countError = true; }
                if (loot.GuaranteedLootTable[index].Weight < 0) { _weightError = true; }
            }
            if (_prefabError == true) { EditorGUILayout.HelpBox("One of the List Items does not have ''Item To Drop'' assigned, which will cause an error if it is drawn", MessageType.Error, true); }
            if (_countError == true) { EditorGUILayout.HelpBox("One of the List Items has an incorrect number of items, which will result in items not appearing when drawn", MessageType.Warning, true); }
            if (_weightError == true) { EditorGUILayout.HelpBox("One of the List Items has an incorrect Weight, this will cause erroneous data readings or the whole system will crash", MessageType.Error, true); }
        }
}

/* --------------------- */
// Custom Property Draw
/* --------------------- */

[CustomPropertyDrawer(typeof(DropChangeItem))]
public class BossSpawnDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var weightRectLabel = new Rect(position.x, position.y, 55, 18);
        var weightRect = new Rect(position.x, position.y + 20, 55, 18);

        EditorGUI.LabelField(weightRectLabel, "Weight");
        EditorGUI.PropertyField(weightRect, property.FindPropertyRelative("Weight"), GUIContent.none);

        var ItemRectLabel = new Rect(position.x + 60, position.y, position.width - 60, 18);
        var ItemRect = new Rect(position.x + 60, position.y + 20, position.width - 60 - 75, 18);

        EditorGUI.LabelField(ItemRectLabel, "Object To Spawn");
        EditorGUI.PropertyField(ItemRect, property.FindPropertyRelative("Drop"), GUIContent.none);

        var MinMaxRectLabel = new Rect(position.x + position.width - 70, position.y, 70, 18);

        var MinRect = new Rect(position.x + position.width - 70, position.y + 20, 30, 18);
        var MinMaxRect = new Rect(position.x + position.width - 39, position.y + 20, 9, 18);
        var MaxRect = new Rect(position.x + position.width - 30, position.y + 20, 30, 18);

        EditorGUI.LabelField(MinMaxRectLabel, "Min  -  Max");
        EditorGUI.PropertyField(MinRect, property.FindPropertyRelative("MinCountItem"), GUIContent.none);
        EditorGUI.LabelField(MinMaxRect, "-");
        EditorGUI.PropertyField(MaxRect, property.FindPropertyRelative("MaxCountItem"), GUIContent.none);

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 40;
    }
}

#endif
