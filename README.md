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
     แก้โดยย้ายการ Subscribe ไปไว้ใน Start()

    GameTime เคยพึ่ง static data จาก TimeManager
     ปรับให้รับข้อมูลผ่าน constructor เพื่อให้แยก dependency ออกจากกัน

## Date 25 / 2 / 2569 
               Project setup, folder structure, Unity version check, Add simple assets. Time : 10 min  
               Design TimeManager data structure & event system : 1 hr.
               Implement TimeManager + AdvanceTime logic : 1 hr 30 min
               Implement TimeHopTrigger collider component  : 20 min
               Build TimeUIDisplay HUD  : 15 min
               Testing, bug fixes : 30 min
               Total : 3 hr 45 min.
               
               
## System 2: Combat System

# Overview

    ระบบ Combat ใช้สำหรับให้ Player ต่อสู้กับศัตรูประเภท Slime.Player ใช้ Wand ยิง Projectile ในแนวราบ โดยแต่ละครั้งจะใช้ Arcane Power (AP)

    หาก AP หมด จะไม่สามารถยิงได้ จนกว่าจะ:

    -เวลาเปลี่ยน (Time Hop)
    -Player ออกจาก Combat Area

# โครงสร้างหลัก

    IDamageable
    Interface สำหรับ object ที่รับ damage ได้ ทำให้ Projectile ไม่ต้องรู้จัก Slime โดยตรง
    รองรับการเพิ่ม enemy ใหม่ในอนาคตโดยไม่ต้องแก้โค้ดเดิม

    PlayerCombat
    จัดการ input การยิง, AP, HP
    และ subscribe OnTimeChanged จาก TimeManager เพื่อ reset AP เมื่อเวลาเปลี่ยน

    Projectile
    ขยับตัวด้วย Rigidbody2D
    เรียก TakeDamage() เมื่อชน IDamageable
    และทำลายตัวเองหลัง 3 วินาทีหากไม่ชนอะไร

    Slime
    ใช้ State Machine แบบง่าย 3 state:
    Idle → Chase → Attack
    เมื่อ HP หมด จะ spawn Slime ขนาดเล็ก 2 ตัว (HP 5, scale ครึ่งหนึ่ง)
    Slime ที่ถูก split จะไม่ split ซ้ำอีก

    CombatAreaTrigger
    Reset AP เมื่อ Player เดินออกจากพื้นที่ต่อสู้

    CombatUIDisplay
    แสดง HP และ AP ของ Player ด้วย Slider + ตัวเลข

    การเชื่อมกับ Time Hop

    PlayerCombat subscribe event OnTimeChanged โดยตรง

    เมื่อเวลาเปลี่ยน AP จะถูก reset โดยไม่ต้องให้ TimeManager รู้จักระบบ Combat
    เป็น dependency แบบทางเดียว (Combat ฟัง Time แต่ Time ไม่รู้จัก Combat)

## Assumptions

    AP เริ่มต้นที่ 100

    ยิง 1 ครั้ง ใช้ AP 10

    Slime ใหญ่ HP 20

    Slime เล็ก HP 5

    Slime เล็กที่ split ออกมาจะไม่ split ซ้ำ

    Projectile ยิงตามทิศที่ Player หันเท่านั้น

## Challenges

    Projectile ชน Player ตัวเอง
    → แก้โดยเช็ค CompareTag("Player")

    Slime เล็ก split วนไม่จบ
    → เพิ่ม flag _isSplitSlime เพื่อป้องกันการ split ซ้ำ


## Date 26 / 2 / 2569 
                Design Combat Data structure : 20 min.
                Implement IDamageable interface : 5 min
                Implement PlayerCombat + AP system : 1 hr
                Implement Projectile + UI : 20 min
                Total : 1 hr 45 min.

## Date 27 / 2 / 2569 
                Implement Slime enemy : 1 hr
                Implement combat area triger : 5 min
                Total : 1 hr 5 min.               


## Overall Total : 6 hr 35 min