using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
    {
        // �������� ���� �� �ִ� Ÿ�Ե��� IDamageable�� ����ϰ� OnDamage �޼��带 �ݵ�� �����ؾ� �Ѵ�
        // OnDamage �޼���� �Է����� ������ ũ��(damage), ���� ����(hitPoint), ���� ǥ���� ����(hitNormal)�� �޴´�
        void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
    }

