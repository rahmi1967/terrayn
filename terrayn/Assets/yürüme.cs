using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yürüme : MonoBehaviour
{
    CharacterController controller;                       //Karakter kontrolcüsü bileşenine erişmek için bir referans tanımlar.

    public float movementSpeed = 1f;                      //Karakterin hareket hızını ayarlamak için bir değişken tanımlar.
    public float rotationSpeed = 10f;                     //Karakterin dönüş hızını ayarlamak için bir değişken tanımlar.

    Animator anim;                                        //Karakterin animasyon kontrolcüsüne erişmek için bir referans tanımlar.
    private Transform cam; // Kamera transformunu tutmak için bir değişken tanımladık



    private void Start()
    {
        anim = GetComponent<Animator>();                 //Script'in bağlı olduğu oyun nesnesinin Animator bileşenine erişir ve anim değişkenine atar.
        controller = GetComponent<CharacterController>();// Script'in bağlı olduğu oyun nesnesinin CharacterController bileşenine erişir ve controller değişkenine atar.
        cam = Camera.main.transform; // Kameranın transformunu alıyoruz
    }
    private void Update()
    {

        float hzInput = Input.GetAxisRaw("Horizontal"); //"Horizontal" adlı bir giriş ekseni üzerinde kullanıcının yatay (sol-sağ) girişini alır ve hzInput değişkenine atar. Değer -1 ile 1 arasında olabilir.
        float vInput = Input.GetAxisRaw("Vertical"); //"Vertical" adlı bir giriş ekseni üzerinde kullanıcının dikey (ileri-geri) girişini alır ve vInput değişkenine atar. Değer -1 ile 1 arasında olabilir.

        if (hzInput != 0 || vInput != 0) //Kullanıcının yatay veya dikey girişi olduğunu kontrol eder.
        {
            //anim.SetFloat("Walk", 1f); //Animator bileşeninde "Walk" adlı bir float parametreye, 1 değerini atayarak yürüme animasyonunu başlatır.
            ////Girdi bilgisine göre hedef dönüşü hesaplama
            //float targetRotation = Mathf.Atan2(hzInput, vInput)*Mathf.Rad2Deg; //Kullanıcının girişine bağlı olarak hedef dönüş açısını hesaplar. Mathf.Atan2() işlevi, yatay ve dikey değerler arasındaki açıyı radyan cinsinden hesaplar ve Mathf.Rad2Deg ile dereceye dönüştürür.

            ////Karakterin döndüğü yöne yumuşatılmış geçiş sağlama
            //Quaternion targetQuaternion = Quaternion.Euler(0, targetRotation, 0); //Hedef dönüş açısını kullanarak bir hedef Quaternion oluşturur. Euler açıları kullanarak bir Quaternion oluştururken, yalnızca y dönüşünü (targetRotation) kullanır.
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, rotationSpeed * Time.deltaTime); //Karakterin dönüşünü yumuşatılmış bir geçişle hedef dönüşe doğru yapar. Quaternion.Slerp() işlevi, mevcut dönüş (transform.rotation) ile hedef dönüş (targetQuaternion) arasında yumuşak bir geçiş sağlar. rotationSpeed * Time.deltaTime değeri, dönüş hızını zamanla çarparak düşük FPS durumlarında da düzgün bir geçiş sağlar.
            ////Hedef dönüşüne göre yön hesaplama
            //Vector3 moveDirection = targetQuaternion * Vector3.forward; // Hedef dönüşe göre bir hareket yönü hesaplar. Vector3.forward, (0, 0, 1) vektörünü temsil eder ve yerel z eksenine doğru bir hareketi ifade eder. Hedef dönüş ile bu vektörü çarparak, karakterin dönüş yönünde hareket etmesini sağlar.

            //controller.Move(moveDirection*movementSpeed*Time.deltaTime); //Karakter kontrolcüsünü kullanarak karakteri belirtilen yönde hareket ettirir. moveDirection vektörünü movementSpeed ile çarparak hareket hızını belirler ve Time.deltaTime ile zaman ölçeğine göre düzenler.
            anim.SetFloat("Walk", 1f);

            // Kamera yönünü alarak karakterin dönüşünü hesapla
            Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;// Scale, iki vektörün elemanlarını çarpan bir işlem yapar.
            Vector3 moveDirection = (vInput * camForward + hzInput * cam.right).normalized;

            // Karakteri yürüt
             controller.Move(moveDirection * movementSpeed * Time.deltaTime);

            // Karakterin yönünü kameraya döndür
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            anim.SetFloat("Walk", 0); // Kullanıcının girişi yoksa, "Walk" adlı float parametreye 0 değerini atayarak yürüme animasyonunu durdurur.
        }

        //Quaternion = Dört bileşene sahiptir : (x, y, z, w). İlk üç bileşen (x, y, z) bir vektörü temsil ederken, dördüncü bileşen (w) ise döndürme işleminin büyüklüğünü ve yönlendirilmesini belirler. 
        //Quaternion.Slerp = İki quaternion arasında yumuşak bir geçiş sağlar. İki nokta arasında düz bir çizgi yerine, bir yay üzerinde yumuşak bir geçiş gerçekleştirir.
        
    }


}
