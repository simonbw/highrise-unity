using UnityEngine;

public class PlayerController : MonoBehaviour
{

  public GunScript gun = null;

  void Update()
  {
    // TODO: Is this the best way to be checking input?
    if (Input.GetMouseButtonDown(0) && gun != null)
    {
      gun.Fire();
    }
  }

  void FixedUpdate()
  {
    Rigidbody2D body = GetComponent<Rigidbody2D>();

    Vector2 moveDirection = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    float angle = Vector2.SignedAngle(Vector2.right, moveDirection);
    float speedPercent = Mathf.Clamp01(moveDirection.magnitude);
    GetComponent<WalkController>().WalkTowards(angle, speedPercent);

    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    float targetAngle = Vector2.SignedAngle(Vector2.right, mousePosition - body.position);
    float headAngle = Mathf.Clamp(targetAngle - body.rotation, -90f, 90f);

    GetComponent<AimController>().targetAngle = targetAngle;
    GetComponent<BodyPoser>().headAngle = headAngle;
  }


  //   onTick(dt: number) {
  //     const { body, weapon } = this.human;
  //     [this.sprite.x, this.sprite.y] = body.position;
  //     this.sprite.rotation = body.angle;

  //     this._stanceAngle = stepToward(
  //       this._stanceAngle,
  //       this.getTargetStanceAngle(),
  //       dt * STANCE_ROTATE_SPEED
  //     );

  //     const targetStanceOffset = this.getTargetStanceOffset();
  //     this._stanceOffset[0] = stepToward(
  //       this._stanceOffset[0],
  //       targetStanceOffset[0],
  //       dt * STANCE_ADJUST_SPEED
  //     );
  //     this._stanceOffset[1] = stepToward(
  //       this._stanceOffset[1],
  //       targetStanceOffset[1],
  //       dt * STANCE_ADJUST_SPEED
  //     );
  //   }

  //   onRender(dt: number) {
  //     super.onRender(dt);

  //     const weapon = this.human.weapon;
  //     if (weapon && this.weaponSprite) {
  //       const pushOffset = this.getPushOffset();
  //       if (weapon instanceof MeleeWeapon) {
  //         this.weaponSprite.visible = weapon.currentCooldown <= 0;
  //         this.weaponSprite.position.set(
  //           ...V(weapon.swing.restPosition).iadd([pushOffset, 0])
  //         );
  //       } else {
  //         this.weaponSprite.position.set(
  //           ...weapon.getCurrentHoldPosition().iadd([pushOffset, 0])
  //         );
  //         this.weaponSprite.rotation = weapon.getCurrentHoldAngle() + pushOffset;
  //       }
  //     }
  //   }

  //   getTargetStanceAngle(): number {
  //     if (this.human.weapon instanceof Gun) {
  //       return this.human.weapon.stats.stanceAngle;
  //     } else {
  //       return 0;
  //     }
  //   }

  //   getPosition() {
  //     return this.human.getPosition();
  //   }

  //   getAngle() {
  //     return this.human.getDirection();
  //   }

  //   getTargetStanceOffset(): [number, number] {
  //     if (this.human.weapon instanceof Gun) {
  //       return this.human.weapon.stats.stanceOffset;
  //     } else {
  //       return [0, 0];
  //     }
  //   }

  //   getStanceAngle() {
  //     return this._stanceAngle;
  //   }

  //   getStanceOffset() {
  //     return this._stanceOffset;
  //   }

  //   getRecoilOffset(gun: Gun): number {
  //     return -0.125 * gun.getCurrentRecoilAmount() ** 1.5;
  //   }

  //   getHandPositions(): [V2d, V2d] {
  //     const { weapon } = this.human;
  //     const pushOffset = this.getPushOffset();
  //     if (weapon) {
  //       const [left, right] = weapon.getCurrentHandPositions();
  //       return [left.iadd([pushOffset, 0]), right.iadd([pushOffset, 0])];
  //     } else {
  //       // Wave em in the air like you just don't care?
  //       let x: number = 0.3 + pushOffset;
  //       const y = Math.sin(this.game!.elapsedTime * 2) * 0.05;
  //       return [V(x, -0.2 + y), V(x, 0.2 - y)];
  //     }
  //   }

  //   getPushOffset(): number {
  //     const pushPhase = this.human.pushAction.currentPhase?.name;
  //     const t = smoothStep(this.human.pushAction.phasePercent);

  //     switch (pushPhase) {
  //       case undefined:
  //         return 0;
  //       case "windup":
  //         return lerp(0, -0.2, t);
  //       case "push":
  //         return lerp(-0.2, 0.2, t);
  //       case "winddown":
  //         return lerp(0.2, 0, t);
  //       default:
  //         return 0;
  //     }
  //   }

  //   async onGiveWeapon(weapon: Gun | MeleeWeapon) {
  //     if (weapon instanceof Gun) {
  //       const { textures, muzzleLength } = weapon.stats;
  //       this.weaponSprite = Sprite.from(textures.holding);
  //       this.weaponSprite.scale.set(GUN_SCALE);
  //       this.weaponSprite.anchor.set(0.5, 0.5);
  //       this.weaponSprite.position.set(...weapon.getCurrentHoldPosition());
  //       this.sprite.addChild(this.weaponSprite);

  //       if (weapon.stats.laserSightColor) {
  //         this.laserSight = this.addChild(
  //           new LaserSight(
  //             () => this.getMuzzlePosition(),
  //             () => weapon.getCurrentHoldAngle() + this.sprite.rotation,
  //             undefined,
  //             weapon.stats.laserSightColor
  //           )
  //         );
  //       }
  //     } else if (weapon instanceof MeleeWeapon) {
  //       const { pivotPosition, textures, size } = weapon.stats;
  //       const { restAngle, restPosition } = weapon.swing;

  //       this.weaponSprite = Sprite.from(textures.hold);
  //       this.weaponSprite.scale.set(size[1] / this.weaponSprite.height);
  //       this.weaponSprite.anchor.set(...pivotPosition);
  //       this.weaponSprite.rotation = Math.PI / 2 + restAngle;
  //       this.weaponSprite.position.set(...restPosition);
  //       this.sprite.addChild(this.weaponSprite);
  //     }
  //   }

  //   getMuzzlePosition() {
  //     const gun = this.human.weapon;
  //     if (gun instanceof Gun) {
  //       const localPosition = gun.getMuzzlePosition();
  //       localPosition.angle += this.sprite.rotation;
  //       return localPosition.iadd([this.sprite.x, this.sprite.y]);
  //     }
  //     return V(0, 0);
  //   }

  //   onDropWeapon() {
  //     if (this.weaponSprite) {
  //       this.sprite.removeChild(this.weaponSprite);
  //       this.weaponSprite = undefined;
  //     }
  //     this.laserSight?.destroy();
  //     this.laserSight = undefined;
  //   }
}


// export function stepToward(from: number, to: number, stepSize: number): number {
//   if (to > from) {
//     return Math.min(from + stepSize, to);
//   } else {
//     return Math.max(from - stepSize, to);
//   }
// }