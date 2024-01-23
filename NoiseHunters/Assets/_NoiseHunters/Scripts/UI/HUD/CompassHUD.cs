using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassHUD : MonoBehaviour
{
    [Header("HUD Depended Components")]
    private ThirdPersonController player;
    private CharacterCombat playerCombat;
    private Animator animator;

    [Header("Compass Values")]
    [SerializeField] private string Animator_ActivateCompass;
    [SerializeField] private RawImage compassImage;
    [SerializeField] private List<ObjectiveMarker> objectivesMarkers;
    private float compassUnit;


    public void SetPlayer(ThirdPersonController thisPlayer)
    {
        player = thisPlayer;
        playerCombat = thisPlayer.GetComponent<CharacterCombat>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        compassUnit = compassImage.rectTransform.rect.width / 360f;
    }

    private void Update()
    {
        if (compassImage != null)
        {
            compassImage.uvRect = new Rect(player.c_CharacterCamera.c_CharacterCamera.transform.localEulerAngles.y / 360, 0f, 1, 1);

            for (int i = 0; i < objectivesMarkers.Count; i++)
            {
                objectivesMarkers[i].markerImage.rectTransform.anchoredPosition = ObjectiveMarkerPosInCompass(objectivesMarkers[i]);
            }
        }
    }

    public void AddMarker(GameObject markerPrefab, Objective objectiveToMark, int objectiveIndex)
    {
        ObjectiveMarker newMarker = Instantiate(markerPrefab, compassImage.transform).GetComponent<ObjectiveMarker>();
        newMarker.givenObjective = objectiveToMark;
        newMarker.objectiveIndex = objectiveIndex;
        newMarker.SetColor(newMarker.givenObjective.objective.questMarkerColor);
        objectivesMarkers.Add(newMarker);
    }
    public void RemoveMarker(Objective objective, int objectiveIndex)
    {
        for (int i = 0; i < objectivesMarkers.Count; i++)
        {
            if (objectivesMarkers[i].givenObjective == objective)
            {
                if (objectivesMarkers[i].objectiveIndex == objectiveIndex)
                {
                    objectivesMarkers[i].RemoveMarker();
                    objectivesMarkers.Remove(objectivesMarkers[i]);
                    break;
                }
            }
        }
    }

    private Vector2 ObjectiveMarkerPosInCompass(ObjectiveMarker marker)
    {
        Vector2 playerPos = new Vector2(player.c_CharacterCamera.transform.position.x, player.c_CharacterCamera.transform.position.z);
        Vector2 playerForward = new Vector2(player.c_CharacterCamera.transform.forward.x, player.c_CharacterCamera.transform.forward.z);

        Vector2 objectivePos = new Vector2(marker.givenObjective.objectivePositions[marker.objectiveIndex].position.x, marker.givenObjective.objectivePositions[marker.objectiveIndex].position.z);

        float angleInCompass = Vector2.SignedAngle(objectivePos - playerPos, playerForward);

        return new Vector2(compassUnit * angleInCompass, 0f);
    }

    public void ShowCompass()
    {
        animator.SetTrigger(Animator_ActivateCompass);
    }
}
