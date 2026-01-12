using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardDisplay : MonoBehaviour
{
    public Text playerVoteText;
    public Text rivalVoteText1;
    public Text rivalVoteText2;
    public Text rivalVoteText3;

    public Image firstMedal; // Medal images
    public Image secondMedal;
    public Image thirdMedal;
    public Image fourthMedal;

    void Start()
    {
        // Retrieve votes from PlayerPrefs and display only the vote counts
        if (playerVoteText != null)
        {
            playerVoteText.text = PlayerPrefs.GetInt("PlayerVotes", 0).ToString();
        }

        if (rivalVoteText1 != null)
        {
            rivalVoteText1.text = PlayerPrefs.GetInt("Rival1Votes", 0).ToString();
        }

        if (rivalVoteText2 != null)
        {
            rivalVoteText2.text = PlayerPrefs.GetInt("Rival2Votes", 0).ToString();
        }

        if (rivalVoteText3 != null)
        {
            rivalVoteText3.text = PlayerPrefs.GetInt("Rival3Votes", 0).ToString();
        }

        // Update the medals after setting up the vote counts
        RearrangeMedals();
    }

    void RearrangeMedals()
    {
        // Get vote counts
        int playerVoteCount = PlayerPrefs.GetInt("PlayerVotes", 0);
        int rivalVoteCount1 = PlayerPrefs.GetInt("Rival1Votes", 0);
        int rivalVoteCount2 = PlayerPrefs.GetInt("Rival2Votes", 0);
        int rivalVoteCount3 = PlayerPrefs.GetInt("Rival3Votes", 0);

        // Create a list of vote counts and their associated Text transforms
        List<KeyValuePair<int, Transform>> positions = new List<KeyValuePair<int, Transform>>
        {
            new KeyValuePair<int, Transform>(playerVoteCount, playerVoteText.transform),
            new KeyValuePair<int, Transform>(rivalVoteCount1, rivalVoteText1.transform),
            new KeyValuePair<int, Transform>(rivalVoteCount2, rivalVoteText2.transform),
            new KeyValuePair<int, Transform>(rivalVoteCount3, rivalVoteText3.transform)
        };

        // Sort the positions by vote counts in descending order
        positions.Sort((a, b) => b.Key.CompareTo(a.Key));

        // Offset to place medals to the right of the corresponding Text fields
        Vector3 offset = new Vector3(200, 0, 0); // Adjust for spacing

        // Assign medals to the corresponding top scores
        firstMedal.transform.position = positions[0].Value.position + offset;
        secondMedal.transform.position = positions[1].Value.position + offset;
        thirdMedal.transform.position = positions[2].Value.position + offset;
        fourthMedal.transform.position = positions[3].Value.position + offset;
    }
}

