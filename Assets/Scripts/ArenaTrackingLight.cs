using UnityEngine;

public class ArenaTrackingLight : MonoBehaviour
{
    const float damp = 0.5f;
    private Quaternion BaseRotation { get; } = Quaternion.Euler(0, 0, 0);

    public void TurnOn()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void TurnOff()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void TrackBot(BotBase bot)
    {
        TurnOn();
        Quaternion rotate = Quaternion.LookRotation(bot.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime);
    }
    public void ResetSelf()
    {
        TurnOff();
        transform.rotation = BaseRotation;
    }
}
