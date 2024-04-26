using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IDatabase
{
    
    public void Create();

    public void Setup();
    public void Save();

    public void Load();


}

public class Database : MonoBehaviour
{
    public IDatabase database;

    
    private void Start()
    {
        //업캐스팅 database는 Firebase객체를 가리키지만 database는 IDatabase 타입이기 때문에 IDatabase 멤버에만 접근이 가능함
        //데이터베이스 종류 고른후함.
        database = new FirebaseManager();
        //database = new AWS();
        //IDatabase룰 상속받는 자식의 타입에 따라 자신만의 함수로 다르게 실행 
        database.Setup();
    }
    public void Btn_Save()
    {
        Save();
    }

    public void Btn_Load()
    {
        Load();
    }

    public void Create()
    {
        PlayerManager playerManager = Manager.ins.playerManager;
        playerManager.ShowPlayerSetupPanel();

        database.Create();
    }

    public void Save()
    {
        database.Save();
    }

    public void Load()
    {
        database.Load();
    }
}
