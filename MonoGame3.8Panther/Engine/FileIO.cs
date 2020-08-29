﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System;
using Panther;

namespace Asteroids2020.Engine
{
    class FileIO : GameComponent
    {
        #region Fields
        FileStream fileStream;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public FileIO(Game game) : base(game)
        {

            game.Components.Add(this);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();

        }

        public void LoadContent()
        {

        }

        public void BeginRun()
        {

        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Write array of vertices for Vector Model.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="fileName"></param>
        public void WriteFile(Vector3[] vertices, string fileName)
        {
            fileName = "Content/Models/" + fileName + ".vec";
            fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

            foreach (Vector3 vert in vertices)
            {
                byte[] verticy = new UTF8Encoding(true).GetBytes(vert.ToString());
                fileStream.Write(verticy, 0, verticy.Length);
            }

            Close();
        }
        /// <summary>
        /// Read model file and convert/decode to Vector3 Array.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Vector3[] ReadFile(string fileName)
        {
            string dataRead = "";
            List<Vector3> vertRead = new List<Vector3>();
            fileName = "Content/Models/" + fileName + ".vec";

            if (File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                byte[] dataByte = new byte[1024];
                UTF8Encoding bufferUTF8 = new UTF8Encoding(true);

                while (fileStream.Read(dataByte, 0, dataByte.Length) > 0)
                {
                    dataRead += bufferUTF8.GetString(dataByte, 0, dataByte.Length);
                }

                Close();

                string onAxis = "";
                string number = "";
                float X = 0;
                float Y = 0;
                float Z;

                foreach (char dataChar in dataRead)
                {
                    if (dataChar.ToString() == "'\0'")
                    {
                        break;
                    }

                    if (dataChar.ToString() == "{" || dataChar.ToString() == ":")
                    {
                        continue;
                    }

                    switch (dataChar.ToString())
                    {
                        case "X":
                            onAxis = dataChar.ToString();
                            continue;
                        case "Y":
                            onAxis = dataChar.ToString();
                            continue;
                        case "Z":
                            onAxis = dataChar.ToString();
                            continue;
                    }

                    switch (onAxis)
                    {
                        case "X":
                            if (dataChar.ToString() == " ")
                            {
                                X = float.Parse(number);
                                number = "";
                                onAxis = "";
                            }
                            else
                            {
                                if (dataChar.ToString() == "}")
                                {
                                    continue;
                                }

                                number += dataChar.ToString();
                            }
                            break;
                        case "Y":
                            if (dataChar.ToString() == " ")
                            {
                                Y = float.Parse(number);
                                number = "";
                                onAxis = "";
                            }
                            else
                            {
                                if (dataChar.ToString() == "}")
                                {
                                    continue;
                                }

                                number += dataChar.ToString();
                            }
                            break;
                        case "Z":
                            if (dataChar.ToString() == "}")
                            {
                                Z = float.Parse(number);
                                vertRead.Add(new Vector3(X, Y, Z));
                                onAxis = "";
                                number = "";
                            }
                            else
                            {
                                number += dataChar.ToString();
                            }
                            break;
                    }
                }
            }
            else
            {
                //Debug file not found.
                System.Diagnostics.Debug.WriteLine("File " + fileName + " not found.");
                vertRead.Add(Vector3.Zero);
                vertRead.Add(Vector3.Zero);
            }

            Vector3[] verts = new Vector3[vertRead.Count];

            for (int i = 0; i < vertRead.Count; i++)
            {
                verts[i] = vertRead[i];
            }

            return verts;
        }

        void Close()
        {
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
        }
        #endregion
        #region Private Methods
        #endregion
    }
}