
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace disusdev
{

public class HUDSystem : Singleton<HUDSystem>
{

  public enum IndicatorType
  {
    Damage,
    Ammo
  }

  [System.Serializable]
  public struct ParinModule
  {
    public GameObject Object;
    public GameObject[] PanelsButtons;
    public GameObject[] PanelsApproved;

    public void Activate()
    {
      for (int i = 0; i < PanelsButtons.Length; i++)
      {
        PanelsButtons[i].SetActive(true);
        PanelsApproved[i].SetActive(false);
      }
      Object.SetActive(true);
    }

    public int Step()
    {
      if (!Object.activeSelf) return 0;
      int approved_count = 0;
      for (int i = 0; i < PanelsButtons.Length; i++)
      {
        if (PanelsApproved[i].activeSelf)
        {
          approved_count++;
          continue;
        }

        if (i >= Gamepad.all.Count) break;

        if (Gamepad.all[i].leftShoulder.isPressed &&
            Gamepad.all[i].rightShoulder.isPressed)
        {
          SfxPlayer.Instance.PlaySfx(SfxPlayer.SfxType.Select);
          PanelsButtons[i].SetActive(false);
          PanelsApproved[i].SetActive(true);
        }
      }

      return approved_count;
    }

    public void Hide()
    {
      Object.SetActive(false);
    }
  }

  [System.Serializable]
  public struct TextTimer
  {
    public GameObject Object;
    public TMP_Text Text;
    private float timer;

    private bool is_event_done;
    public float EventTime;

    public UnityEvent OnTimerEnd;
    public UnityEvent OnTimerEvent;

    private bool active;

    public void Start(float time)
    {
      Object.SetActive(true);
      timer = time;
      Text.text = timer.ToString("0");
      active = true;
      is_event_done = false;
    }

    public void Stop()
    {
      Object.SetActive(false);
      timer = 0.0f;
      Text.text = timer.ToString("0");
      active = false;
    }

    public void Step(float dt)
    {
      if (!active) return;

      timer -= dt;

      if (!is_event_done && timer < EventTime)
      {
          is_event_done = true;
          OnTimerEvent.Invoke();
      }

      if (timer < 0)
      {
        timer = 0.0f;
        active = false;
        Object.SetActive(false);
        OnTimerEnd.Invoke();
      }
      Text.text = timer.ToString("0");
    }
  }

  [System.Serializable]
  public struct BarIndicator
  {
    private Transform glued;
    private Vector2 offset;

    public RectTransform pos_trans;
    public Slider slider;

    public Image bg;
    public Image hp;

    public float hide_speed;

    public void Play(Vector2 position, int value)
    {
      glued = null;
      pos_trans.position = Camera.main.WorldToScreenPoint(position);

      slider.value = value;

      timer = 1.0f;

      Color color = hp.color;
      color.a = 1.0f;
      hp.color = color;

      color = bg.color;
      color.a = 0.5f;
      bg.color = color;
    }

    public void PlayGlued(Transform parrent, Vector2 offset, int value)
    {
      glued = parrent;
      this.offset = offset;

      pos_trans.position = Camera.main.WorldToScreenPoint((Vector2)glued.position + this.offset);
      
      slider.value = value;

      timer = 1.0f;

      Color color = hp.color;
      color.a = 1.0f;
      hp.color = color;

      color = bg.color;
      color.a = 0.5f;
      bg.color = color;
    }

    private float timer;
    public void Update(float dt)
    {
      timer -= dt * hide_speed;
      if (timer < 0.0f)
      {
          timer = 0.0f;
      }

      Color color = hp.color;
      color.a = timer;
      hp.color = color;

      color = bg.color;
      color.a = timer - 0.5f;
      bg.color = color;

      if (glued != null)
      {
        this.offset += Vector2.up * dt;
        pos_trans.position = Camera.main.WorldToScreenPoint((Vector2)glued.position + this.offset);
      }
      else
      {
        pos_trans.localPosition += Vector3.up * timer;
      }
    }
  }

  [System.Serializable]
  public struct TextIndicator
  {
    private Transform glued;
    private Vector2 offset;

    public TMP_Text text;
    public RectTransform trans;
    public float hide_speed;
    public bool avaliable;

    private Color color;
    public void Play(Vector2 position, string value, Color color)
    {
      glued = null;
      trans.position = Camera.main.WorldToScreenPoint(position);
      text.text = value;
      timer = 1.0f;

      this.color = color;

      this.color.a = 1.0f;
      text.color = this.color;

      avaliable = false;
    }

    public void PlayGlued(Transform parrent, Vector2 offset, string value, Color color)
    {
      glued = parrent;
      this.offset = offset;

      trans.position = Camera.main.WorldToScreenPoint((Vector2)glued.position + this.offset);
      text.text = value;
      timer = 1.0f;

      this.color = color;

      this.color.a = 1.0f;
      text.color = this.color;

      avaliable = false;
    }

    private float timer;
    public void Update(float dt)
    {
      timer -= dt * hide_speed;
      if (timer < 0.0f)
      {
          timer = 0.0f;
          avaliable = true;
      }

      color.a = timer;
      text.color = color;

      if (glued != null)
      {
        this.offset += Vector2.up * dt;
        trans.position = Camera.main.WorldToScreenPoint((Vector2)glued.position + this.offset);
      }
      else
      {
        trans.localPosition += Vector3.up * timer;
      }
    }
  }

  public TextIndicator[] Indicators;
  public BarIndicator[] BarIndicators;

  public void DrawIndicator(Vector2 position, string value, Color color)
  {
    for (int i = 0; i < Indicators.Length; i++)
    {
      if (Indicators[i].avaliable)
      {
        Indicators[i].Play(position, value, color);
        return;
      }
    }
  }

  public void DrawGluedIndicator(Transform parrent, Vector2 offset, string value, Color color)
  {
    for (int i = 0; i < Indicators.Length; i++)
    {
      if (Indicators[i].avaliable)
      {
        Indicators[i].PlayGlued(parrent, offset, value, color);
        return;
      }
    }
  }

  public void DrawGluedBarIndicator(int index, Transform parrent, Vector2 offset, int value)
  {
    BarIndicators[index].PlayGlued(parrent, offset, value);
  }

  private Color TypeToColor(IndicatorType type)
  {
    switch (type)
    {
      case IndicatorType.Damage:
      return Color.red;
      case IndicatorType.Ammo:
      return Color.yellow;
    }
    return Color.white;
  }

  public void DrawIndicator(Vector2 position, string value, IndicatorType type)
  {
    for (int i = 0; i < Indicators.Length; i++)
    {
      if (Indicators[i].avaliable)
      {
        Indicators[i].Play(position, value, TypeToColor(type));
        return;
      }
    }
  }

  public void DrawGluedIndicator(Transform parrent, Vector2 offset, string value, IndicatorType type)
  {
    for (int i = 0; i < Indicators.Length; i++)
    {
      if (Indicators[i].avaliable)
      {
        Indicators[i].PlayGlued(parrent, offset, value, TypeToColor(type));
        return;
      }
    }
  }

  public TextTimer Timer;

  public void StartTimer(float time, UnityAction on_end, float event_time, UnityAction on_event)
  {
    Timer.OnTimerEnd.RemoveAllListeners();
    Timer.OnTimerEnd.AddListener(on_end);
    Timer.OnTimerEvent.RemoveAllListeners();
    Timer.OnTimerEvent.AddListener(on_event);
    Timer.EventTime = event_time;

    Timer.Start(time);
  }

  public void ActiveWinnerPanel(string player_name)
  {
    WinnerPanel.SetActive(true);
    WinnerPlayerText.text = player_name;
  }

  public GameObject PlayButton;
  public GameObject WinnerPanel;
  public TMP_Text WinnerPlayerText;

  public ParinModule Pair;

  public void Step(float dt)
  {
    for (int i = 0; i < Indicators.Length; i++)
    {
      Indicators[i].Update(dt);
    }

    for (int i = 0; i < BarIndicators.Length; i++)
    {
      BarIndicators[i].Update(dt);
    }

    Timer.Step(dt);

    int paired_players = Pair.Step();

    bool bypass = false;
    bool submit = false;

    if (Gamepad.all.Count > 0)
    {
      bypass = Gamepad.current.startButton.wasPressedThisFrame;// Input.GetButtonDown("bypass");
      submit = Gamepad.current.aButton.wasPressedThisFrame;
    }

    if (((paired_players == 4) || bypass) && Pair.Object.activeSelf == true)
    {
      SfxPlayer.Instance.PlaySfx(SfxPlayer.SfxType.Click);
      Pair.Hide();
      PlayButton.SetActive(true);

      GameStateManager.Instance.SpawnPlayers(paired_players);
    }    

    if (submit && PlayButton.activeSelf == true)
    {
      SfxPlayer.Instance.PlaySfx(SfxPlayer.SfxType.Click);
      PlayButton.SetActive(false);
      GameStateManager.Instance.StartGame();
    }

    if (submit && WinnerPanel.activeSelf == true)
    {
      SfxPlayer.Instance.PlaySfx(SfxPlayer.SfxType.Select);
      WinnerPanel.SetActive(false);
      // PlayButton.SetActive(true);
      Pair.Activate();
      GameStateManager.Instance.LoadScene(0);
    }
  }

  //private void OnGUI()
  //{
  //  bool l0 = Input.GetButton(Pair.l_buttons[0]);
  //  bool r0 = Input.GetButton(Pair.r_buttons[0]);

  //  bool l1 = Input.GetButton(Pair.l_buttons[1]);
  //  bool r1 = Input.GetButton(Pair.r_buttons[1]);

  //  bool l2 = Input.GetButton(Pair.l_buttons[2]);
  //  bool r2 = Input.GetButton(Pair.r_buttons[2]);

  //  bool l3 = Input.GetButton(Pair.l_buttons[3]);
  //  bool r3 = Input.GetButton(Pair.r_buttons[3]);

  //  string log = string.Format("l0: {0}\nr0: {1}\nl1: {2}\nr1: {3}\nl2: {4}\nr2: {5}\nl3: {6}\nr3: {7}\n", l0, r0, l1, r1, l2, r2, l3, r3);
  //  GUI.TextArea(new Rect(0.0f, 0.0f, 200.0f, 400.0f), log);
  //}
}

}
