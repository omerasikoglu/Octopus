using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

//Verili event yollama
public class Notes : MonoBehaviour
{
    public static Notes Instance { get; private set; }
    private MinionTypeSO activeMinionType;

    public event EventHandler<OnActiveMinionTypeChangedEventArgs> OnActiveMinionTypeChanged;

    public class OnActiveMinionTypeChangedEventArgs : EventArgs
    {
        public MinionTypeSO activeMinionType;
    }
    public void SetActiveMinionType(MinionTypeSO minionType)
    {
        activeMinionType = minionType;
        OnActiveMinionTypeChanged?.Invoke(this,
            new OnActiveMinionTypeChangedEventArgs { activeMinionType = activeMinionType });
    }
    private void Start()
    {
        Instance = this;
    }
}
public class Notes2 : MonoBehaviour
{
    private void Start()
    {
        Notes.Instance.OnActiveMinionTypeChanged += MinionManager_OnActiveMinionTypeChanged;
    }
    private void MinionManager_OnActiveMinionTypeChanged(object sender, Notes.OnActiveMinionTypeChangedEventArgs e)
    {                                                        //(object sender, System.EventArgs e)
        if (e.activeMinionType == null)
        {
            Hide();
        }
        else
        {
            Show(e.activeMinionType.sprite);
        }
    }
    private void Hide() { }
    private void Show(Sprite st) { }
}

//Resource işlemleri
public class Notes3 : MonoBehaviour
{
    public ResourceTypeListSO resourceTypeList;

    public void Start()
    {
        //olan kaynaktan çekme
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        //Instantiate
        Transform pfEnemy = Resources.Load<Transform>("pfEnemy");
        Transform enemyTransform = Instantiate(pfEnemy, transform.position, Quaternion.identity);
    }


}

#region CompareTo() Örneği
//namespace ConsoleNotes
//{
//    public class Test
//    {
//        public static void Main()
//        {
//            string isim = "Hasan";
//            string[] collection = { "Ali", "Hasan", "Yasan", "Basan" };

//            foreach (string st in collection)
//            {
//                int value = isim.CompareTo(st);

//                Console.Write("Hasanın gücü ");
//                if (value < 0)
//                {
//                    Console.Write($"{st}'dan az\n");
//                }
//                else if (value == 0)
//                {
//                    Console.Write($"{st}'a eşit\n");
//                }
//                else if (value > 0)
//                {
//                    Console.Write($"{st}'dan daha çoh\n");
//                }
//            }
//        }
//    }
//}
#endregion


//get-set
public class Notes4 : MonoBehaviour
{
    public static Notes4 Instance { get; private set; }

    /******************************     AÇILMIŞ HALİ    ******************************/

    public static Notes4 instance;

    public static Notes4 GetInstance()
    {
        return instance;
    }
    private static void SetInstance(Notes4 set)
    {
        instance = set;
    }

    //      ---YA DA---
    //private static T instance;

    //public static T Instance
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //}
}


//tooltip 5 sn açık kal
class Notes5 : MonoBehaviour
{
    private TooltipTimer2 tooltipTimer;
    private void Update()
    {
        if (tooltipTimer != null)
        {
            tooltipTimer.timer -= Time.deltaTime;
            if (tooltipTimer.timer <= 0)
            {
                //tooltip kapan
            }
        }
    }
    public void Show(string st, TooltipTimer2 tooltipTimer = null)
    {
        //tooltip açıl ve 5 sn açık kal
        this.tooltipTimer = tooltipTimer;
    }


    public class TooltipTimer2
    {
        public float timer;
    }
    //dışardan çağırması:
    //
    //.Instance.Show(new TooltipUI.TooltipTimer { timer = 2f });
}
//           

// QUEST MANAGER CALISAN İLK HALİ
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class StarManager : MonoBehaviour
//{
//    public event System.EventHandler OnStarAchieved;

//    [SerializeField] StarTypeSO starType;
//    [SerializeField] bool isAchieved = false;
//    private SpriteRenderer spriteRenderer;
//    [SerializeField] private LevelSO currentLevel;


//    private void Awake()
//    {

//        spriteRenderer = GetComponent<SpriteRenderer>();
//        spriteRenderer.sprite = starType.sprite;

//    }
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.transform.CompareTag("Player"))
//        {
//            int index = 0;
//            foreach (StarTypeSO starType in currentLevel.starTypesList)
//            {
//                if (starType == this.starType)
//                {
//                    QuestManager.Instance.SetStarAchieved(index);
//                }
//                index++;
//            }
//            Destroy(gameObject);
//        }
//    }

//}

#region Bi Clamp Kullanımı
//currentEnergy = Mathf.Clamp(currentEnergy + Time.deltaTime, 0, maxEnergy);
#endregion


#region Dic
class Dic : MonoBehaviour
{
    Dictionary<string, string> dic;
    private void Start()
    {
        dic = new Dictionary<string, string>();
        dic.Add("asd", "asd2");

        Console.WriteLine(dic["asd"]); // asd2'yi yazdırır; valur değerini verir
        Console.WriteLine(dic.ContainsKey("asd")); // key'ler arasında "asd" varsa true ya da false döndürür
    }
}




#endregion

//< RectTransform > ().localPosition-- > posx
//<RectTransform>().sizeDelta-- > width height-- > alttaki

#region Try-Catch-Finally-Dispose


class DisposeClassOrnek : MonoBehaviour
{
    public void St()
    {
        string path = "";
        using (FileStream fileStreem = new FileStream(path, FileMode.Open))
        {
            //kodlar
        }

        /**** AŞAĞIDAKİYLE AYNI ŞEY *************************************************************/

        FileStream fileStream = new FileStream(path, FileMode.Create);
        try
        {
            //kodlar
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            if (fileStream != null)((IDisposable)fileStream).Dispose();
        }
    }

}

#endregion



#region Command patterni update içinde basılınca çalışacak kısım
//buttonW.Execute(); 
#endregion

#region Input GetKey true/false
//wallGrabbing = Input.GetKey(KeyCode.Z);

    //alttaki yerine üstteki daha optimize

//if (Input.GetKey(KeyCode.Z))
//{
//    wallGrabbing = true;
//}
#endregion