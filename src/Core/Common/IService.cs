/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016/11/14
 * Time: 14:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public interface IService : IDisposable
    {
        int ServerStatus();

        bool StartServer(object tag = null);

        void StopServer();

    }
}
