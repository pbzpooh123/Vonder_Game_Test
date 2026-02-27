# Vonder_Game_Test
> Unity Version: **2022.3.13 LTS**

---

# System 1: Time Hop System
 ## Overview
 ระบบ Time Hop นี้ทำหน้าที่จัดการเวลาในเกมแบบง่าย ๆ โดยแบ่ง 1 วันออกเป็น 3 ช่วงเวลา ได้แก่

    - Morning  
    - Afternoon  
    - Evening  

    ลำดับการเปลี่ยนเวลาจะเป็น:

    Morning → Afternoon → Evening → วันถัดไป (Morning)

    ระบบวันจะวนเป็นรายสัปดาห์ (Monday → Sunday) และสามารถนับวันเพิ่มไปได้เรื่อย ๆ แบบไม่จำกัด

    การทดสอบ Time Hop ใช้วิธีเดินชน Trigger Collider ภายในฉาก เพื่อให้ทดสอบระบบได้ง่าย

## โครงสร้างหลักแบ่งออกเป็น:

    GameTime
    ใช้เก็บข้อมูลเวลาในรูปแบบ immutable struct (snapshot ของเวลา ณ ตอนนั้น)

    TimeManager
    ทำหน้าที่จัดการ state ของเวลา และควบคุมการเปลี่ยนช่วงเวลา

    TimeConfig
    เป็น ScriptableObject สำหรับเก็บชื่อช่วงเวลาและชื่อวัน เพื่อให้แก้ไขได้จาก Inspector โดยไม่ต้องแก้โค้ด

    TimeUIDisplay
    แสดงผลเวลาใน HUD โดย subscribe event จาก TimeManager

    TimeHopTrigger
    ทำหน้าที่ตรวจจับการชนของ Player แล้วเรียก AdvanceTime()

    ผมเลือกใช้ event (OnTimeChanged) แทนการให้ UI ไปเช็คค่าเวลาเองทุก frame เพื่อแยกความรับผิดชอบของแต่ละระบบออกจากกัน

## Assumptions

    เริ่มต้น Day 1 = Monday

    เวลาเปลี่ยนเฉพาะเมื่อชน Trigger เท่านั้น (ไม่มี real-time clock)

    ยังไม่ได้ทำระบบ save/load เวลา

## ปัญหาที่พบระหว่างทำ

    UI Subscribe event ก่อนที่ TimeManager จะถูก initialize
    → แก้โดยย้ายการ Subscribe ไปไว้ใน Start()

    GameTime เคยพึ่ง static data จาก TimeManager
    → ปรับให้รับข้อมูลผ่าน constructor เพื่อให้แยก dependency ออกจากกัน

## Date 25 / 2 / 2569 
               Project setup, folder structure, Unity version check, Add simple assets. Time : 10 min  
               Design TimeManager data structure & event system : 1 hr.
               Implement TimeManager + AdvanceTime logic : 1 hr 30 min
               Implement TimeHopTrigger collider component  : 20 min
               Build TimeUIDisplay HUD  : 15 min
               Testing, bug fixes : 30 min
               Total : 3 hr 45 min.
               
               
# System 2: Combat System





## Date 26 / 2 / 2569 
                Design Combat Data structure : 20 min.
                Implement IDamageable interface : 5 min
                Implement PlayerCombat + AP system : 1 hr
                Implement Projectile + UI : 20 min
                Total : 1 hr 45 min.

## Date 27 / 2 / 2569 
                Implement Slime enemy : 1 hr
                
                Total : 1 hr min.               