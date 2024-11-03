
using UnityEngine;
[CreateAssetMenu(fileName = "IceAndFireEffect", menuName = "Data/ItemEffect/IceAndFire")]
public class ItemEffect_IceAndFire : ItemEffect
{
    [SerializeField] private float xVelocity;
    [SerializeField]private GameObject icefirePrefab;
    private Player player;
    private int AttackCounter;
    public override void doItemEffect()
    {
        player=PlayerManager.instance.player;
        AttackCounter=player.AttackState.comboCounter;
        if (AttackCounter == 2)
        {
            GameObject icefire=Instantiate(icefirePrefab,player.transform.position,Quaternion.identity);
            IceAndFireController controller=icefire.GetComponent<IceAndFireController>();
            controller.SetUp(player, xVelocity);
        }

    }

}
