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
            Debug.Log("数据库连接成功");
            connection.Close();
        }
        catch
        {
            Debug.Log("数据库连接失败");
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
                loginTitle.text = "登录成功";
                StartCoroutine(ChangeScene());
            }
            else
            {
                loginTitle.text = "登录失败，账号或密码有误";
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
                loginTitle.text = "注册成功";
            }
            else
            {
                loginTitle.text = "账号或密码不能为空！";
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
