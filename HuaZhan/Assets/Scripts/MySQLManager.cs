using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MySQLManager : MonoBehaviour
{
    public string server = "127.0.0.1";
    public int port = 3306;
    public string user = "root";
    public string password = "qq2915721770";
    public string database = "xxxx";
    private string connectionString;

    public InputField userInput;
    public InputField passwordInput;
    public Text loginTitle;

    public InputField setUserInput;
    public InputField setPasswordInput;
    //public Text regsiterTitle;

    private MySqlConnection connection;

    private void Start()
    {
        connectionString = $"server={server};port={port};user={user};password={password};database={database};charset=latin1;";
        connection = new MySqlConnection(connectionString);
        /*try
        {
            connection.Open();
            Debug.Log("???????????");
            connection.Close();
        }
        catch
        {
            Debug.Log("????????????");
        }*/
    }

    public void Login()
    {
        try
        {
            connection.Open();
            string user1 = userInput.text.Trim(); ;
            string password1 = passwordInput.text.Trim(); ;

            MySqlCommand command = new MySqlCommand("select * from zhanghao where user=@user and password=@password", connection);
            command.Parameters.AddWithValue("user", user1);
            command.Parameters.AddWithValue("password", password1);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                #region agent log
                DebugSessionLogger.Log("run1", "H4", "MySQLManager.cs:57", "Login success branch", $"user={user1}");
                #endregion
                UserSession.SetCurrentUser(user1);
                #region agent log
                DebugSessionLogger.Log("run1", "H4", "MySQLManager.cs:61", "Before reload current user progress", "calling ExhibitionProgressManager.Instance.ReloadForCurrentUser");
                #endregion
                ExhibitionProgressManager.Instance.ReloadForCurrentUser();
                loginTitle.text = "\u767b\u5f55\u6210\u529f";
                StartCoroutine(ChangeScene());
            }
            else
            {
                loginTitle.text = "\u767b\u5f55\u5931\u8d25\uff0c\u8d26\u53f7\u6216\u5bc6\u7801\u6709\u8bef";
            }
        }
        finally { connection.Close(); }
    }

    public void Register()
    {
        try
        {
            connection.Open();
            string setUser1 = setUserInput.text.Trim();
            string setPassword1 = setPasswordInput.text.Trim();
            if (setUser1 != null && setPassword1 != null&&setUser1 !="" && setPassword1 != "")
            {
                MySqlCommand command = new MySqlCommand("insert into zhanghao set user=@SetUser,password=@SetPassword",connection);
                command.Parameters.AddWithValue("SetUser", setUser1);
                command.Parameters.AddWithValue("SetPassword", setPassword1);
                command.ExecuteNonQuery();
                loginTitle.text = "\u6ce8\u518c\u6210\u529f";
            }
            else
            {
                loginTitle.text = "\u8d26\u53f7\u6216\u5bc6\u7801\u4e0d\u80fd\u4e3a\u7a7a\uff01";
            }
        }
        finally
        {
            connection.Close();
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("showroom");
    }
}
