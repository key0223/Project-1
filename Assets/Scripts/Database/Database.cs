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
        //��ĳ���� database�� Firebase��ü�� ����Ű���� database�� IDatabase Ÿ���̱� ������ IDatabase ������� ������ ������
        //�����ͺ��̽� ���� ������.
        database = new FirebaseManager();
        //database = new AWS();
        //IDatabase�� ��ӹ޴� �ڽ��� Ÿ�Կ� ���� �ڽŸ��� �Լ��� �ٸ��� ���� 
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
