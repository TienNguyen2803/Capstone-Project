using BusinessLogic.Define;
using CapstoneUI.Controllers;
using CapstoneUI.ViewModels;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.Firebase
{
  public class FirebaseDatabase
  {
    private const String databaseUrl = "https://capstone-spsc.firebaseio.com/";

    private const String databaseSecret = "BJ8ERP4fHv0NKSu3v6uJYLgTwZ1TCQRzGTP5R6K4";
    private FirebaseClient client;
    private static FirebaseDatabase _instance;
    private IAccountService _accountService;
    private IRoleService _roleService;
    // Lock synchronization object

    private static object syncLock = new object();

    // Constructor (protected)
    protected FirebaseDatabase()
    {
      this.client = new FirebaseClient(
        databaseUrl,
        new FirebaseOptions
        {
          AuthTokenAsyncFactory = () => Task.FromResult(databaseSecret)
        });
    }

    public FirebaseDatabase(IAccountService accountService, IRoleService roleService)
    {
      _accountService = accountService;
      _roleService = roleService;
    }

    public static FirebaseDatabase GetFirebaseDatabase()
    {
      // Support multithreaded applications through

      // 'Double checked locking' pattern which (once

      // the instance exists) avoids locking each

      // time the method is invoked

      if (_instance == null)
      {
        lock (syncLock)
        {
          if (_instance == null)
          {
            _instance = new FirebaseDatabase();
          }
        }
      }
      return _instance;
    }

    public async Task sendMessage(FireBaseVM viewModel)
    {
      viewModel.IsReading = false;
      var role = viewModel.Role;
      var messageTopic = role;

      var mess = (await client
        .Child(messageTopic)
        .OnceAsync<FirebaseMessageModel>()).FirstOrDefault(_ => _.Object.status == (int)viewModel.Status && _.Object.month == viewModel.Month
        && _.Object.year == viewModel.Year);
      if (mess != null)
      {
        if (mess.Object.isReading)
        {
          await client
          .Child(messageTopic).
          Child(mess.Key)
          .PutAsync(
          JsonConvert.SerializeObject(new FirebaseMessageModel(
            viewModel.Role, viewModel.IsReading, viewModel.CreatedDate, (int)viewModel.Status, viewModel.Count, viewModel.Month, viewModel.Year, viewModel.DocCode)));
        }
      }
      else
      {
        var obj = await client
          .Child(messageTopic)
          .PostAsync(
          JsonConvert.SerializeObject(new FirebaseMessageModel(
            viewModel.Role, viewModel.IsReading, viewModel.CreatedDate, (int)viewModel.Status, viewModel.Count, viewModel.Month, viewModel.Year, viewModel.DocCode)));
      }
      // default sending message
     
      
    }
   
  }
}
