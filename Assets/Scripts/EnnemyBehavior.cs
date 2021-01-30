﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyBehavior : MonoBehaviour
{
    NavMeshAgent ag;
    GameObject player;
    Vector3 target;
    private bool following = false;
    public float dist_monster = 10f;
    PlayerMove player_script;
    private List<Vector3> pattern = new List<Vector3>();
    int current_index = 0;
    private bool begin = false;
    private float dist_target;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        target = player.transform.position;
        ag = GetComponent<NavMeshAgent>();
        player_script = player.GetComponent<PlayerMove>();
        pattern.Add(new Vector3(9,0,9));
        pattern.Add(new Vector3(2,0,2));
        dist_target = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((target - transform.position).magnitude);
        RaycastHit hit;
        float angle = Vector3.Angle(transform.forward,player.transform.position-transform.position);
        Physics.Raycast(gameObject.transform.position, -(gameObject.transform.position - player.transform.position).normalized, out hit, Mathf.Infinity);
        Debug.DrawRay(transform.position, -(gameObject.transform.position - player.transform.position).normalized * hit.distance, Color.yellow);
        if (!following && !player_script.hide && Vector3.Distance(player.transform.position, transform.position) <= dist_monster && hit.collider.gameObject.tag == "Player" && angle <= 45)
        {
            ag.SetDestination(player.transform.position);
            target = player.transform.position;
            following = true;
        }
        else if (!begin) {
            target = pattern[current_index];
            ag.SetDestination(target);
            dist_target = transform.position.y;
            begin = true;
        }
        else if (following && (player_script.hide || (hit.collider.gameObject.tag != "Player" && angle >= 45))) { //on arrête de follow
            ag.SetDestination(pattern[current_index]);
            target = pattern[current_index];
            following = false;
        }
        else if (following && (target - transform.position).magnitude <= 1f) {
            ag.SetDestination(player.transform.position);
            target = player.transform.position;
        }
        else if (!following && (target - transform.position).magnitude <= dist_target+0.2f) {
            current_index = (current_index + 1) % pattern.Count;
            ag.SetDestination(pattern[current_index]);
            target = pattern[current_index];
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player") {
            Debug.Log("T'es mort");
        }
    }
}
