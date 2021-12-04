using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInput : MonoBehaviour
{
    GameObject target;
    [SerializeField] float mouse_sensitivity = 0.2f;
    [SerializeField] float touch_sensitivity = 0.001f;

    GameObject[] childobjects= new GameObject[9]; //回転させるCubeを格納する配列

    string[,,] rubic= new string[3,3,3]; //ルービックキューブを表す配列
    string[,,] crubic= new string[3,3,3]; //回転させた後書き換える用の配列

    int i,j,k,ti,tj,tk,n=1;

    void Start()
    {
        //1. 3×3×3のstring型の3次元配列を用意し、rubic1~27をそれぞれ入れる
        for(i=0;i<3;i++){
            for(j=0;j<3;j++){
                for(k=0;k<3;k++){
                    rubic[i,j,k] = "rubic"+n;
                    crubic[i,j,k] = "rubic"+n;
                    n++;
                }
            }
        }
    }
 
    void Update()
    {
        //タッチしたオブジェクトを取得する
        if(Input.GetMouseButtonDown(0)){
            target = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if(Physics.Raycast(ray, out hit)){
                target = hit.collider.gameObject;

                //2.オブジェクトがどこにあるか探索
                for(i=0;i<3;i++){
                    for(j=0;j<3;j++){
                        for(k=0;k<3;k++){
                            if(target.name ==  rubic[i,j,k]){
                                ti=i;
                                tj=j;
                                tk=k;  
                            }
                        }
                    }
                }
            }
        }

        //ドラッグ中もしくはスワイプ中
        if (Input.GetMouseButtonUp(0))
        {
            float dx,dy;
            Vector3 point;
            
            //マウスの場合
            dx = Input.GetAxis("Mouse X") * mouse_sensitivity;
            dy = Input.GetAxis("Mouse Y") * mouse_sensitivity;
 
            //タッチの場合
            if (Input.touchCount > 0)
            {
                dx = Input.touches[0].deltaPosition.x * touch_sensitivity;
                dy = Input.touches[0].deltaPosition.y * touch_sensitivity;
            }

            if(dx > 0.001f || dx < -0.001f){//左or右回転
                //rubic[i,j,k]をすべて取得する iはrayで取得したもので固定
                i=0;
                for(j=0;j<3;j++){
                    for(k=0;k<3;k++){
                        childobjects[i] = GameObject.Find(rubic[ti,j,k]);
                        i++;
                    }
                }
                //実際に回転させる
                point = childobjects[4].transform.position;
                for(i=1;i<9;i++){
                        childobjects[i].transform.SetParent(childobjects[0].transform);
                }

                if(dx < -0.001f){//右回転
                    childobjects[0].transform.RotateAround(point,Vector3.up,90);
                    for(j=0;j<3;j++){
                        for(k=0;k<3;k++){
                            rubic[ti,j,k] =crubic[ti,3-k-1,j];
                        }
                    }
                }
                else if(dx > 0.001f){//左回転
                    childobjects[0].transform.RotateAround(point,Vector3.down,90);
                    for(j=0;j<3;j++){
                        for(k=0;k<3;k++){
                             rubic[ti,j,k] =crubic[ti,k,3-1-j];
                        }
                    }
                }

                for(i=1;i<9;i++){
                    childobjects[i].transform.parent= null;
                }
                //3次元配列を書き換える
 
                for(i=0;i<3;i++){
                    for(j=0;j<3;j++){
                        for(k=0;k<3;k++){
                            crubic[i,j,k] = rubic[i,j,k];
                        }
                     }
                }
            }

            else if(dy > 0.001f || dy < -0.001f){//上or下回転
                k=0;
                for(i=0;i<3;i++){
                    for(j=0;j<3;j++){
                        childobjects[k] = GameObject.Find(rubic[i,j,tk]);
                        k++;
                    }
                }
                point = childobjects[4].transform.position;
                for(i=1;i<9;i++){
                        childobjects[i].transform.SetParent(childobjects[0].transform);
                }

                if(dy > 0.001f){//上
                    childobjects[0].transform.RotateAround(point,Vector3.back,90);
                    for(i=0;i<3;i++){
                        for(j=0;j<3;j++){
                            rubic[i,j,tk] =crubic[3-j-1,i,tk];
                        }
                    }
                }

                else if(dy < -0.001f){//下
                childobjects[0].transform.RotateAround(point,Vector3.forward,90);
                    for(i=0;i<3;i++){
                        for(j=0;j<3;j++){
                            rubic[i,j,tk] =crubic[j,3-i-1,tk];
                        }
                    }
                }
                
                for(i=1;i<9;i++){
                    childobjects[i].transform.parent= null;
                }
                for(i=0;i<3;i++){
                    for(j=0;j<3;j++){
                        for(k=0;k<3;k++){
                            crubic[i,j,k] = rubic[i,j,k];
                        }
                     }
                }
            }

        }
    }
}